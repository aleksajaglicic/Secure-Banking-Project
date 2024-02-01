using Common.Interface;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BankingAudit
{
    public class DetectionManagement : IDetectionManagement
    {
        public void DetectWithdrawText(string username, string period, string numOfTransactions, string serviceName, string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Bank {serviceName} detected transaction warning");
            Console.WriteLine("--------------------------");
            Console.WriteLine("| Transactional Warning. |");
            Console.WriteLine("--------------------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> Writing log...\n");

            DetectAudit.WithdrawDetection(username, value, serviceName, numOfTransactions, period);
        }
    }
}
