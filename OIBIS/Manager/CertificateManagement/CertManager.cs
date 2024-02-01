using System;
using System.Configuration;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Manager.CertManagement
{
    public class CertManager
    {
        #region Get Certificate

        /// <summary>
        /// Get a certificate with the specified subject name from the predefined certificate storage
        /// Only valid certificates should be considered
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="storeLocation"></param>
        /// <param name="subjectName"></param>
        /// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
        public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly); //// 

            /// Check whether the subjectName of the certificate is exactly the same as the given "subjectName"
            foreach (X509Certificate2 c in (X509Certificate2Collection)store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true))
            {
                if (c.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a certificate from file.		
        /// </summary>
        /// <param name="username">.pfx file name</param>
        /// <param name="pin"> password for .pfx file</param>
        /// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
        public static X509Certificate2 GetCertificateFromFile(string username, string pin)
        {
            string filePath = $"Certificates\\{username}.pfx";

            string pass = pin;

            X509Certificate2Collection collection = new X509Certificate2Collection();
            collection.Import(filePath, pass, X509KeyStorageFlags.PersistKeySet); // ako promeni pin a ne insatalira novi cert ovde puca
            foreach (X509Certificate2 cert in collection)
            {
                return cert;
            }
            return null;
        }

        /// <summary>
        /// Get a certificate from file.
        /// </summary>
        /// <param name="fileName">.pfx file name</param>
        /// <param name="pwd"> password for .pfx file</param>
        /// <returns>The requested certificate. If no valid certificate is found, returns null.</returns>
        public static X509Certificate2 GetCertificateFromFile(string fileName, SecureString pwd)
        {
            X509Certificate2 certificate = null;
            return certificate;
        }

        #endregion

        #region Generate Certificate

        public static string GeneratePin()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 10000).ToString();
        }
        public static string GenerateCertificate(string username, string pincode)
        {
            string serviceCert = ConfigurationManager.AppSettings["ServiceCertificate"];
            string pin = pincode.Equals("") ? GeneratePin() : pincode;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            byte[] pvkBytes = rsa.ExportCspBlob(true);

            string pvk = Convert.ToBase64String(pvkBytes);

            X509Certificate2 issuerCertificate = CertManager.GetCertificateFromStorage(StoreName.Root,
                                                                                       StoreLocation.LocalMachine,
                                                                                       serviceCert);

            issuerCertificate.Extensions.Add(new X509BasicConstraintsExtension(true, false, 0, false));

            CertificateRequest request = new CertificateRequest($"CN={username}",
                                                                rsa,
                                                                HashAlgorithmName.SHA256,
                                                                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign | X509KeyUsageFlags.KeyEncipherment, false));
            
            byte[] serial = new byte[20];
            Random rnd = new Random();
            rnd.NextBytes(serial);
            
            using (X509Certificate2 cert = request.Create(issuerCertificate, DateTimeOffset.UtcNow,
                                                            DateTimeOffset.UtcNow.AddYears(1),
                                                            serial))
            {
                RSACryptoServiceProvider cryptoProvider = DecodeRsaPrivateKey(pvkBytes);
                cert.PrivateKey = cryptoProvider;

                byte[] certBytes = cert.Export(X509ContentType.Pfx, pin);

                System.IO.File.WriteAllBytes($"Certificates/{username}.pfx", certBytes);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("---------------------------------");
                Console.WriteLine("| User certificate saved.\t|");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("| Certificate Details:\t\t|");
                Console.WriteLine("| Subject: " + cert.Subject + "\t\t|");
                Console.WriteLine("| Issuer: " + cert.Issuer + "\t\t|");
                Console.WriteLine("---------------------------------");
                Console.ResetColor();

            }
            return pin;
        }
        private static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privateKeyBytes)
        {
            CspParameters cspParams = new CspParameters
            {
                ProviderType = 1,
                KeyContainerName = Guid.NewGuid().ToString(),
                KeyNumber = 1
            };

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams);
            rsa.ImportCspBlob(privateKeyBytes);
            return rsa;
        }

        #endregion
    }
}
