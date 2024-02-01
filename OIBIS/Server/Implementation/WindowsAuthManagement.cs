using Common;
using Common.Interface;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Runtime.ConstrainedExecution;

namespace Server.Implementation
{
    public class WindowsAuthManagement : IWindowsAuthManagement
    {
        public void RevokeCertificate(string username, string pin)
        {
            Manager.CertManagement.RevocationManager.RevokeCertificate(username, pin);
        }
        public byte[] DataBackup()
        {
            byte[] serializedData = Manager.DataManager.SerializeDictionary(Common.InMemoryDatabase.Database.Users);
            return serializedData;
        }

        public bool CheckUsersPin(string username, string pinCode)
        {
            return Common.InMemoryDatabase.Database.CheckPin(username, pinCode);
        }

        public string RevokedCertsBackup()
        {
            string data = Manager.DataManager.FormatListData(Manager.CertManagement.RevocationManager.RevokedCertificates);
            return data;
        }
        public void test()
        {
            Console.WriteLine("Test win auth.");
        }
    }
}
