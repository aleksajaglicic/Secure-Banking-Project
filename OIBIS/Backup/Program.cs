using Common;
using Common.Interface;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceModel;


namespace Backup
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Backup Server started by: " + WindowsIdentity.GetCurrent().Name;

            #region ABC

            string serviceAddressW = "net.tcp://localhost:8001/IWindowsAuthManagement";

            NetTcpBinding bindingW = new NetTcpBinding();
            bindingW.Security.Mode = SecurityMode.Transport;
            bindingW.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingW.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ChannelFactory<IWindowsAuthManagement> channelBackup =
               new ChannelFactory<IWindowsAuthManagement>(bindingW, serviceAddressW);
            IWindowsAuthManagement backupProxy = channelBackup.CreateChannel();

            #endregion

            string input = "";

            while (true)
            {
                Console.WriteLine("Backup Server is started.\n");
                PrintMenu();
                input = Console.ReadLine();

                if (input == "1")
                {
                    byte[] data = backupProxy.DataBackup();
                    Dictionary<string, User> deserializedData = Manager.DataManager.DeserializeDictionary(data);
                    Manager.DataManager.SaveData(deserializedData);

                    Console.WriteLine("Press <Enter> to continue.");
                    Console.ReadLine();
                    Console.Clear();

                }
                else if (input == "2")
                {
                    string revokedData = backupProxy.RevokedCertsBackup();
                    string revokedPath = "Data/revoked_certs.txt";
                    Manager.DataManager.SaveStringToFile(revokedPath, revokedData);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("-------------------------------------------------------------------");
                    Console.WriteLine($"| Revoked certificates have been saved to {revokedPath}. |");
                    Console.WriteLine("-------------------------------------------------------------------");
                    Console.ResetColor();
                    Console.WriteLine("Press <Enter> to continue.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            void PrintMenu()
            {
                Console.WriteLine("------------------------");
                Console.WriteLine("[1] Backup data ");
                Console.WriteLine("[2] Backup revoked certs ");
                Console.WriteLine("------------------------");
                Console.WriteLine(" |");
                Console.Write(" --> ");
            }
        }
    }
}
