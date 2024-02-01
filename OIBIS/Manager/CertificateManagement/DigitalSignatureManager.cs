using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Manager.CertManagement
{
    public enum HashAlgorithm { SHA1, SHA256 }
    public class DigitalSignatureManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"> a message/text to be digitally signed </param>
        /// <param name="hashAlgorithm"> an arbitrary hash algorithm </param>
        /// <param name="certificate"> certificate of a user who creates a signature </param>
        /// <returns> byte array representing a digital signature for the given message </returns>
        public static byte[] Create(string message, HashAlgorithm hashAlgorithm, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;

            if (csp == null)
            {
                throw new Exception("Valid certificate was not found.");
            }

            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(message);
            byte[] hash = null;

            if (hashAlgorithm.Equals(HashAlgorithm.SHA1))
            {
                SHA1Managed sha1 = new SHA1Managed();
                hash = sha1.ComputeHash(data);
            }
            else if (hashAlgorithm.Equals(HashAlgorithm.SHA256))
            {
                SHA256Managed sha256 = new SHA256Managed();
                hash = sha256.ComputeHash(data);
            }

            //Console.WriteLine("Hash: " + BitConverter.ToString(hash).Replace("-", ""));
           
            try
            {
                byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()));

                return signature;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error signing hash: " + ex.Message);
                Console.ResetColor();
                throw;
            }
        }

        public static bool Verify(string message, HashAlgorithm hashAlgorithm, byte[] signature, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(message);
            byte[] hash = null;

            if (hashAlgorithm.Equals(HashAlgorithm.SHA1))
            {
                SHA1Managed sha1 = new SHA1Managed();
                hash = sha1.ComputeHash(data);
            }
            else if (hashAlgorithm.Equals(HashAlgorithm.SHA256))
            {
                SHA256Managed sha256 = new SHA256Managed();
                hash = sha256.ComputeHash(data);
            }

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()), signature);
        }
    }
}
