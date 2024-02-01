using Common;
using Common.Interface;
using Manager.CertManagement;
using Server.Implementation;
using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace Server
{
    class Program
    {
        public static string serviceCert = ConfigurationManager.AppSettings["ServiceCertificate"];

        static void Main(string[] args)
        {

            #region WCF

            string serviceAddressWCF = "net.tcp://localhost:8000/IAccountManagement";
            NetTcpBinding binding = new NetTcpBinding();


            #endregion

            #region AUTH

            string serviceAddressW = "net.tcp://localhost:8001/IWindowsAuthManagement";

            NetTcpBinding bindingW = new NetTcpBinding();
            bindingW.Security.Mode = SecurityMode.Transport;
            bindingW.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingW.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            #endregion

            #region CERT

            string serviceAddressC = "net.tcp://localhost:8002/ICertificateAuthManagement";

            NetTcpBinding bindingC = new NetTcpBinding();
            bindingC.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            bindingC.Security.Mode = SecurityMode.Transport;
            bindingC.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            #endregion

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
           
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            try
            {
                ServiceHost hostWCF = new ServiceHost(typeof(AccountManagement));
                hostWCF.AddServiceEndpoint(typeof(IAccountManagement), binding, serviceAddressWCF);


                hostWCF.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
                hostWCF.Description.Behaviors.Add(newAudit);


                hostWCF.Open();

                ServiceHost hostAuth = new ServiceHost(typeof(WindowsAuthManagement));
                hostAuth.AddServiceEndpoint(typeof(IWindowsAuthManagement), bindingW, serviceAddressW);
                hostAuth.Open();


                ServiceHost hostCer = new ServiceHost(typeof(CertificateAuthManagement));
                hostCer.AddServiceEndpoint(typeof(ICertificateAuthManagement), bindingC, serviceAddressC);

                hostCer.Credentials.ClientCertificate.Authentication.RevocationMode =
                    X509RevocationMode.NoCheck; //

                hostCer.Credentials.ServiceCertificate.Certificate =
                    CertManager.GetCertificateFromStorage(StoreName.Root,
                                                          StoreLocation.LocalMachine, serviceCert);


                hostCer.Credentials.ClientCertificate.Authentication.CertificateValidationMode =
                    X509CertificateValidationMode.ChainTrust;

                hostCer.Open();


                Console.Title = "Server started by: " + WindowsIdentity.GetCurrent().Name;
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("| WCFService for Windows Communication Foundation is started.\t|");
                Console.WriteLine("| WCFService for Windows Authentication is started.\t\t|");
                Console.WriteLine("| WCFService for Certificates is started.\t\t\t|");
                Console.WriteLine("-----------------------------------------------------------------");
                Console.ReadLine();

                hostWCF.Close();
                hostAuth.Close();
                hostCer.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}
