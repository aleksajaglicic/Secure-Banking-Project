using Common;
using Common.Interface;
using Common.Model;
using Manager.CertManagement;
using Manager.CryptographyManagment;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace Server.Implementation
{
    public class CertificateAuthManagement : ICertificateAuthManagement
    {
        public static string serviceCert = ConfigurationManager.AppSettings["ServiceCertificate"];
        string secretKey = SecretKey.LoadKey(ConfigurationManager.AppSettings["SecretKeyPath"]);

        #region PIN
        public bool ResetPIN(string username, string oldPinCode, string newPinCode)
        {

            if (Common.InMemoryDatabase.Database.UpdateUsersPin(username, oldPinCode, newPinCode))
            {
                Console.WriteLine(" |");
                Console.WriteLine($" --> User {username} changed pin");
                CertManager.GenerateCertificate(username, newPinCode);
                Audit.ResetPinSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
                return true;
            }
            Audit.ResetPinFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's certificate revoked");
            return false;
        }
        public bool IsPinValid(string username, string pinCode)
        {
            return Common.InMemoryDatabase.Database.CheckPin(username, pinCode);
        }
        #endregion

        #region Balance
        public int CheckBalance(string username, string pinCode)
        {
            if (Common.InMemoryDatabase.Database.CheckPin(username, pinCode))
            {
                return Common.InMemoryDatabase.Database.Users[username].Balance;
            }

            return -1;
        }
        #endregion

        #region Deposit
        public bool Deposit(string username, string pinCode, string value, byte[] signature)
        {
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, username);

            if (DigitalSignatureManager.Verify(value.ToString(), HashAlgorithm.SHA1, signature, certificate))
            {
                Audit.DepositRequestSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
            }
            else
            {
                Audit.DepositRequestFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's digital signature is invalid");
            }

            int valueDecrypted = Int32.Parse(CryptographyManager.DecryptString(value, secretKey, System.Security.Cryptography.CipherMode.ECB));

            if (!Common.InMemoryDatabase.Database.CheckPin(username, pinCode))
            {
                Audit.DepositFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's pin is invalid");
                return false;
            }

            User user = Common.InMemoryDatabase.Database.Users[username];
            bool depositSuccess = Common.InMemoryDatabase.Database.IncreaseUsersBalance(user, valueDecrypted);

            if (depositSuccess)
            {
                user.Transactions.Add(new Transaction(valueDecrypted, DateTime.Now, 0));
                Audit.DepositSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action, valueDecrypted.ToString());
            }
            else
            {
                Audit.DepositFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "Deposit failed");
            }

            return depositSuccess;
        }
        #endregion

        #region Withdraw
        public bool Withdraw(string username, string pinCode, string value, byte[] signature)
        {
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, username);

            if (DigitalSignatureManager.Verify(value, HashAlgorithm.SHA1, signature, certificate))
            {
                Audit.WithdrawRequestSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
            }
            else
            {
                Audit.WithdrawRequestFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's digital signature is invalid");
            }

            int valueDecrypted = Int32.Parse(CryptographyManager.DecryptString(value, secretKey, System.Security.Cryptography.CipherMode.ECB));


            if (!Common.InMemoryDatabase.Database.CheckPin(username, pinCode))
            {
                Audit.WithdrawFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's pin is invalid");
                return false;
            }

            User user = Common.InMemoryDatabase.Database.Users[username];
            bool withdrawSuccess = Common.InMemoryDatabase.Database.DecreaseUsersBalance(user, valueDecrypted);

            if (withdrawSuccess)
            {
                Audit.WithdrawSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action, valueDecrypted.ToString());
                
                user.Transactions.Add(new Transaction(valueDecrypted, DateTime.Now, 1));

                int periodOfTime = Int32.Parse(ConfigurationManager.AppSettings["periodOfTime"]);
                int numOfTransactions = Int32.Parse(ConfigurationManager.AppSettings["numOfTransactions"]);

                List<Transaction> withdraws = new List<Transaction>();
                foreach(Transaction tr in user.Transactions)
                {
                    if(tr.TypeOfTransaction == 1)
                    {
                        withdraws.Add(tr);
                    }
                }

                TimeSpan period = TimeSpan.FromSeconds(periodOfTime);

                if (withdraws.Count >= numOfTransactions) //10 
                {
                    Transaction fTransaction = withdraws[withdraws.Count - numOfTransactions]; //10 -3 -1 = 7
                    Transaction lTransaction = withdraws[withdraws.Count - 1]; // 10 -1 = 9 

                    TimeSpan timeSpan = (lTransaction.DateTime) - (fTransaction.DateTime);

                    if (timeSpan <= period)
                    {
                        List<Transaction> lastNTransactions = withdraws.GetRange(withdraws.Count - numOfTransactions, numOfTransactions);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        int i = 0;
                        foreach (Transaction t in lastNTransactions)
                        {
                            i++;
                            sb.Append($"{t.Value}");
                            
                            if(i != numOfTransactions)
                            {
                                sb.Append(",");
                            }
                        }
                        sb.Append("]");

                        ReportLog(username, sb.ToString());
                    }
                }
            }
            else
            {
                Audit.WithdrawFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's balance is less than requested amount");
            }
            return withdrawSuccess;
        }
        #endregion

        #region IsCertRevoked
        public bool IsCertRevoked(string username, string pinCode)
        {
            return RevocationManager.IsCertificateRevoked(username, pinCode);
        }
        #endregion

        #region ReportLog
        public void ReportLog(string username, string value)
        {
            string period = ConfigurationManager.AppSettings["periodOfTime"];
            string numOfTransactions = ConfigurationManager.AppSettings["numOfTransactions"];
            string serviceName = WindowsIdentity.GetCurrent().Name;

            string serviceAddressD = "net.tcp://localhost:8003/IDetectWithdraw";

            NetTcpBinding bindingD = new NetTcpBinding();
            bindingD.Security.Mode = SecurityMode.Transport;
            bindingD.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingD.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ChannelFactory<IDetectionManagement> channelDetect =
               new ChannelFactory<IDetectionManagement>(bindingD, serviceAddressD);
            IDetectionManagement detectProxy = channelDetect.CreateChannel();

            detectProxy.DetectWithdrawText(username, period, numOfTransactions, serviceName, value);
        }
        #endregion

        #region Test
        public void test()
        {
            Console.WriteLine("Test cert");
        }
        #endregion
    }
}
