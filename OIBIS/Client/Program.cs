using Client.Interface;
using Common;
using Common.Interface;
using Manager;
using Manager.CertManagement;
using Manager.CryptographyManagment;
using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

namespace Client
{
    public enum Communication { WCF, AUTH, CERT };
    public enum Option { CreateAccount, RevokeCertificate, ResetPIN, Deposit, Withdraw, CheckBalance };

    class Program
    {
        public static string serviceCert = ConfigurationManager.AppSettings["ServiceCertificate"];

        static void Main(string[] args)
        {
            ConsoleInterface.SetConsoleTitle();
            PrintMenu();
        }

        private static void PrintMenu()
        {
            string selectedOption = ConsoleInterface.GetClientOption();

            switch (selectedOption)
            {
                case "1":
                    WCFConnection(Option.CreateAccount);
                    break;
                case "2":
                    CERTConnection(Option.ResetPIN);
                    break;
                case "3":
                    AUTHConnection(Option.RevokeCertificate);
                    break;
                case "4":
                    CERTConnection(Option.Deposit);
                    break;
                case "5":
                    CERTConnection(Option.Withdraw);
                    break;
                case "6":
                    CERTConnection(Option.CheckBalance);
                    break;
                default:
                    PrintMenu();
                    break;
            }
            PrintMenu();
        }
        private static void WCFConnection(Option value)
        {
            #region ABC

            string serviceAddressWCF = "net.tcp://localhost:8000/IAccountManagement";
            NetTcpBinding bindingWCF = new NetTcpBinding();

            ChannelFactory<IAccountManagement> channelWCF =
                new ChannelFactory<IAccountManagement>(bindingWCF, serviceAddressWCF);

            IAccountManagement proxy = channelWCF.CreateChannel();

            #endregion

            switch (value)
            {
                case Option.CreateAccount:
                    Console.Clear();
                    ConsoleInterface.PrintCommunicationAndOption(Communication.WCF, Option.CreateAccount);

                    try
                    {
                        string pin = proxy.CreateAccount(Formatter.ParseName(WindowsIdentity.GetCurrent().Name));
                        if (pin.Equals(""))
                        {
                            ConsoleInterface.PrintError();
                        }
                        else
                        {
                            ConsoleInterface.PinRequest(pin);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
            }
        }
        private static void AUTHConnection(Option value)
        {
            string username = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            #region ABC

            string serviceAddressW = "net.tcp://localhost:8001/IWindowsAuthManagement";
            NetTcpBinding bindingW = new NetTcpBinding();
            bindingW.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingW.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            bindingW.Security.Mode = SecurityMode.Transport;

            ChannelFactory<IWindowsAuthManagement> windowsChannel =
                    new ChannelFactory<IWindowsAuthManagement>(bindingW, serviceAddressW);
            IWindowsAuthManagement windowsProxy = windowsChannel.CreateChannel();

            #endregion

            switch (value)
            {
                case Option.RevokeCertificate:

                    Console.Clear();
                    ConsoleInterface.PrintCommunicationAndOption(Communication.AUTH, Option.RevokeCertificate);

                    try
                    {
                        X509Certificate2 cert = Manager.CertManagement.CertManager.GetCertificateFromStorage(StoreName.My,
                                                                                              StoreLocation.LocalMachine,
                                                                                              username);

                        if (cert == null)
                        {
                            ConsoleInterface.PrintCertNotFound();
                            return;
                        }
                        else
                        {
                            ConsoleInterface.PrintCertInfo(cert);

                            string pinCode = ConsoleInterface.EnterPin();
                            if (!windowsProxy.CheckUsersPin(username, pinCode))
                            {
                                ConsoleInterface.PrintWrongPin();
                                return;
                            }

                            ConsoleInterface.PrintCorrectPin();

                            windowsProxy.RevokeCertificate(username, pinCode);
                            ConsoleInterface.PrintRevokeCertificate();
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    break;
            }

        }
        private static void CERTConnection(Option value)
        {
            string username = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            string secretKey = SecretKey.LoadKey(ConfigurationManager.AppSettings["SecretKeyPath"]);
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            X509Certificate2 clientCertificate = CertManager.GetCertificateFromStorage(StoreName.My,
                                                      StoreLocation.LocalMachine,
                                                      cltCertCN);
            if (clientCertificate == null)
            {
                ConsoleInterface.PrintCertNotFound();
                return;
            }

            #region ABC
            string serviceAddressC = "net.tcp://localhost:8002/ICertificateAuthManagement";

            NetTcpBinding bindingC = new NetTcpBinding();
            bindingC.Security.Mode = SecurityMode.Transport;
            bindingC.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            bindingC.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            EndpointAddress address = new EndpointAddress(new Uri(serviceAddressC),
                                          new X509CertificateEndpointIdentity(CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                                                           StoreLocation.LocalMachine, serviceCert)));

            ChannelFactory<ICertificateAuthManagement> certificateChannel =
                new ChannelFactory<ICertificateAuthManagement>(bindingC, address);

            certificateChannel.Credentials.ServiceCertificate.Authentication.CertificateValidationMode =
                    System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;

            certificateChannel.Credentials.ServiceCertificate.Authentication.RevocationMode =
                    X509RevocationMode.NoCheck; //

            certificateChannel.Credentials.ClientCertificate.Certificate = clientCertificate;

            ICertificateAuthManagement certificateProxy = certificateChannel.CreateChannel();

            #endregion

            switch (value)
            {
                case Option.ResetPIN:
                    Console.Clear();
                    ConsoleInterface.PrintCommunicationAndOption(Communication.CERT, Option.ResetPIN);

                    X509Certificate2 cert = Manager.CertManagement.CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);
                    ConsoleInterface.PrintCertInfo(cert);

                    string oldPin = ConsoleInterface.EnterCurrentPin();

                    if (certificateProxy.IsCertRevoked(username, oldPin))
                    {
                        ConsoleInterface.PrintCertRevoked();
                        return;
                    }
                    ConsoleInterface.PrintCertNotRevoked();

                    if (!certificateProxy.IsPinValid(username, oldPin))
                    {
                        ConsoleInterface.PrintWrongPin();
                        return;
                    }
                    ConsoleInterface.PrintCorrectPin();

                    string newPin = ConsoleInterface.EnterNewPin();

                    if (certificateProxy.ResetPIN(username, oldPin, newPin))
                    {
                        ConsoleInterface.PrintPinChanged();
                    }
                    else
                    {
                        ConsoleInterface.PrintError();
                    }
                    break;
                case Option.Deposit:
                    Console.Clear();
                    ConsoleInterface.PrintCommunicationAndOption(Communication.CERT, Option.Deposit);

                    string pinCode = ConsoleInterface.EnterPin();
                    int balance = 0;

                    if (certificateProxy.IsCertRevoked(username, pinCode))
                    {
                        ConsoleInterface.PrintCertRevoked();
                        return;
                    }

                    ConsoleInterface.PrintCertNotRevoked();

                    if (!certificateProxy.IsPinValid(username, pinCode))
                    {
                        ConsoleInterface.PrintWrongPin();
                        return;
                    }

                    ConsoleInterface.PrintCorrectPin();

                    try
                    {
                        ConsoleInterface.PrintEnterAmount("deposit ");
                        int amount;

                        if (int.TryParse(Console.ReadLine(), out amount))
                        {
                            string amountEncrypted = Manager.CryptographyManagment.CryptographyManager.EncryptString(amount.ToString(), secretKey,
                                                                                System.Security.Cryptography.CipherMode.ECB);
                            byte[] signature = DigitalSignatureManager.Create(amountEncrypted, HashAlgorithm.SHA1, clientCertificate);

                            Interface.ConsoleInterface.TransactionResult(certificateProxy.Deposit(username, pinCode, amountEncrypted, signature));
                            balance = certificateProxy.CheckBalance(username, pinCode);
                            Interface.ConsoleInterface.BalanceRequest(balance);
                        }
                        else
                        {
                            ConsoleInterface.PrintInvalidInput();
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case Option.Withdraw:
                    Console.Clear();
                    ConsoleInterface.PrintCommunicationAndOption(Communication.CERT, Option.Withdraw);

                    pinCode = ConsoleInterface.EnterPin();
                    balance = 0;

                    if (certificateProxy.IsCertRevoked(username, pinCode))
                    {
                        ConsoleInterface.PrintCertRevoked();
                        return;
                    }

                    ConsoleInterface.PrintCertNotRevoked();

                    if (!certificateProxy.IsPinValid(username, pinCode))
                    {
                        ConsoleInterface.PrintWrongPin();
                        return;
                    }

                    ConsoleInterface.PrintCorrectPin();

                    try
                    {
                        ConsoleInterface.PrintEnterAmount("withdraw");

                        int amount;

                        if (int.TryParse(Console.ReadLine(), out amount))
                        {
                            string amountEncrypted = Manager.CryptographyManagment.CryptographyManager.EncryptString(amount.ToString(), secretKey,
                                System.Security.Cryptography.CipherMode.ECB);
                            byte[] signature = DigitalSignatureManager.Create(amountEncrypted, HashAlgorithm.SHA1, clientCertificate);

                            Interface.ConsoleInterface.TransactionResult(certificateProxy.Withdraw(username, pinCode, amountEncrypted, signature));
                            balance = certificateProxy.CheckBalance(username, pinCode);
                            Interface.ConsoleInterface.BalanceRequest(balance);
                        }
                        else
                        {
                            ConsoleInterface.PrintInvalidInput();
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;
                case Option.CheckBalance:
                    Console.Clear();
                    ConsoleInterface.PrintCommunicationAndOption(Communication.CERT, Option.CheckBalance);

                    pinCode = ConsoleInterface.EnterPin();
                    balance = 0;

                    if (certificateProxy.IsCertRevoked(username, pinCode))
                    {
                        ConsoleInterface.PrintCertRevoked();
                        return;
                    }

                    ConsoleInterface.PrintCertNotRevoked();

                    if (!certificateProxy.IsPinValid(username, pinCode))
                    {
                        ConsoleInterface.PrintWrongPin();
                        return;
                    }

                    ConsoleInterface.PrintCorrectPin();

                    try
                    {
                        balance = certificateProxy.CheckBalance(username, pinCode);
                        Interface.ConsoleInterface.BalanceRequest(balance);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                default:
                    ConsoleInterface.PrintInvalidInput();
                    break;
            }
        }
    }
}
