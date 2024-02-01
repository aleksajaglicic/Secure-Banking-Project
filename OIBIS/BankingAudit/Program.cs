using Common.Interface;
using System;
using System.Security.Principal;
using System.ServiceModel;

namespace BankingAudit
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Banking audit server started by: " + WindowsIdentity.GetCurrent().Name;

            #region ABC

            string serviceAddressD = "net.tcp://localhost:8003/IDetectWithdraw";

            NetTcpBinding bindingD = new NetTcpBinding();
            bindingD.Security.Mode = SecurityMode.Transport;
            bindingD.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingD.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            #endregion

            try
            {
                ServiceHost hostDetect = new ServiceHost(typeof(DetectionManagement));
                hostDetect.AddServiceEndpoint(typeof(IDetectionManagement), bindingD, serviceAddressD);
                hostDetect.Open();
                Console.WriteLine("Banking audit service is started.");
                // Console.WriteLine("Press \"Enter\" to stop...");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
