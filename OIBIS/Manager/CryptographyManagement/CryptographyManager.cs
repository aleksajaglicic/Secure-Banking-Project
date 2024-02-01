using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Manager;

namespace Manager.CryptographyManagment
{
    public class CryptographyManager
    {
        public static string EncryptString(string plainText, string secretKey, CipherMode mode)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform tripleDesEncryptTransform = tripleDesCryptoProvider.CreateEncryptor();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesEncryptTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static string DecryptString(string cipherText, string secretKey, CipherMode mode)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = mode,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform tripleDesDecryptTransform = tripleDesCryptoProvider.CreateDecryptor();

            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesDecryptTransform, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
