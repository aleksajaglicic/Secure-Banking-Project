using Common;
using Common.Model;
using Manager;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

namespace Server
{
    internal class AccountManagement : IAccountManagement
    {
        public string GetName()
        {
            return Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
        }

        public string CreateAccount(string username)
        {
            string pin = "";

            try
            {
                if (Common.InMemoryDatabase.Database.GetUser(username) != null)
                {
                    string message = "-----------------------\n";
                    message += "| User already exists |\n";
                    message += "-----------------------";
                    throw new Exception(message);
                }
                else
                {
                    Console.WriteLine(" |");
                    Console.WriteLine($" --> User {username} - CreateAccount ");
                    Manager.CertManagement.RevocationManager.RemoveCertFromList(username);
                    pin = Manager.CertManagement.CertManager.GenerateCertificate(username, "");
                    Common.InMemoryDatabase.Database.AddUser(username, pin);
                    Audit.CreateAccountSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
                }
            }
            catch (Exception e)
            {
                Audit.CreateAccountFailed(username,
                                           OperationContext.Current.IncomingMessageHeaders.Action, "User already exists");
                Console.WriteLine(e.Message);
            }

            return pin;
        }
    }
}


