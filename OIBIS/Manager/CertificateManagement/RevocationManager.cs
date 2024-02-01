using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Manager.CertManagement
{
    public class RevocationManager
    {
        public static List<X509Certificate2> RevokedCertificates = new List<X509Certificate2>();

        public static bool RevokeCertificate(string username, string pin)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromFile(username, pin);

            if (!IsCertificateRevoked(username, pin))
            {
                RevokedCertificates.Add(cert);
                Common.InMemoryDatabase.Database.RemoveUser(username, pin);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--------------------------------");
                Console.WriteLine("| Certificate has been revoked |");
                Console.WriteLine("--------------------------------");
                Console.ResetColor();

                Audit.RevokeCertSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
                return true;
            }
            Audit.RevokeCertFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "User's certificate is already revoked");
            return false;
        }

        public static bool IsCertificateRevoked(string username, string pin)
        {
            foreach(X509Certificate2 cert in RevokedCertificates)
            {
                if(cert.SubjectName.Name.Equals(string.Format("CN={0}", username)))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool RemoveCertFromList(string username)
        {
            foreach (X509Certificate2 cert in RevokedCertificates)
            {
                if (cert.SubjectName.Name.Equals(string.Format("CN={0}", username)))
                {
                    RevokedCertificates.Remove(cert);
                    return true;
                }
            }
            return false;
        }
    }
}
