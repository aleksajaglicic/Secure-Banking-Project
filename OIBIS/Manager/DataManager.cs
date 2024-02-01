using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Manager
{
    public class DataManager
    {
        #region Serialize

        public static byte[] SerializeDictionary(Dictionary<string, User> users)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, users);
                return memoryStream.ToArray();
            }
        }

        public static Dictionary<string, User> DeserializeDictionary(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                IFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, User>)formatter.Deserialize(memoryStream);
            }
        }
        #endregion

        #region Format

        public static string FormatFilePath()
        {
            DateTime dateTime = DateTime.Now;
            string formattedDateTime = dateTime.ToString("dd_MM_yyy_HH_mm_ss");
            return $"Data/data_{formattedDateTime}.txt";
        }

        public static string FormatData(Dictionary<string, User> deserializedData)
        {
            StringBuilder sb = new StringBuilder();

            string userHeader = $"{"| Username ",-16}|{" Balance",-17}|{" Hashed pin",-65} |";
            string userData = "";
            string transactionData = "";
            string transactionHeader = $"{"| Type ",-15}|{" Value",-16} |{" Date",-21} |";

            foreach (User u in deserializedData.Values)
            {
                userData = $"| {u.Username,-13} | {u.Balance,-15} | {u.HashedPin,-15} |";
                sb.AppendLine("------------------------------------------------------------------------------------------------------");
                sb.AppendLine(userHeader);
                sb.AppendLine("------------------------------------------------------------------------------------------------------");
                sb.AppendLine(userData);
                sb.AppendLine(new string('-', userData.Length));
                sb.AppendLine(" |");
                sb.AppendLine(" --> Transactions");
                sb.AppendLine(" ---------------------------------------------------------");
                sb.AppendLine(" " + transactionHeader);
                sb.AppendLine(" ---------------------------------------------------------");
                foreach (Transaction t in u.Transactions)
                {
                    if (t.TypeOfTransaction == 0)
                    {
                        transactionData = $"{" | Deposit",-15} | {t.Value,-15} | {t.DateTime,-15} |";
                    }
                    else if (t.TypeOfTransaction == 1)
                    {
                        transactionData = $"{" | Withdraw",-15} | {t.Value,-15} | {t.DateTime,-15} |";
                    }

                    sb.AppendLine(transactionData);
                    sb.AppendLine(" " + new string('-', transactionData.Length - 1));

                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static string FormatListData(List<X509Certificate2> certs)
        {
            StringBuilder sb = new StringBuilder();
            string header = $"{"| User ",-21}|{" Issuer",-17}|{" Serial Number",-41}   |";
            string data = "";

            sb.AppendLine("-------------------------------------------------------------------------------------");
            sb.AppendLine(header);
            sb.AppendLine("-------------------------------------------------------------------------------------");

            foreach (X509Certificate2 cert in certs)
            {
                data = $"| {cert.SubjectName.Name,-18} | {cert.Issuer,-15} | {cert.SerialNumber,-17} |";
                sb.AppendLine(data);
                sb.AppendLine(new string('-', data.Length));
            }

            return sb.ToString();
        }

        #endregion

        #region Save

        public static void SaveData(Dictionary<string, User> deserializedData)
        {
            string filePath = FormatFilePath();
            string data = FormatData(deserializedData);

            SaveStringToFile(filePath, data);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($"| Data saved to {filePath} |");
            Console.WriteLine("---------------------------------------------------");
            Console.ResetColor();
        }

        public static void SaveStringToFile(string filePath, string data)
        {
            File.WriteAllText(filePath, data);
        }

        #endregion

    }
}
