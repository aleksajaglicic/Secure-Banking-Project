using Manager;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Client.Interface
{
    public class ConsoleInterface
    {
        public static string EnterPin()
        {
            Console.WriteLine("-------------");
            Console.WriteLine("| Enter pin |");
            Console.WriteLine("-------------");
            Console.WriteLine(" |");
            Console.Write(" --> ");
            return Console.ReadLine();
        }
        public static string EnterCurrentPin()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("| Enter current pin |");
            Console.WriteLine("---------------------");
            Console.WriteLine(" |");
            Console.Write(" --> ");
            return Console.ReadLine();
        }
        public static string EnterNewPin()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("| Enter new pin |");
            Console.WriteLine("-----------------");
            Console.WriteLine(" |");
            Console.Write(" --> ");
            return Console.ReadLine();
        }
        public static void PinRequest(string pinCode)
        {
            PrintGenerateCertificate();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-----------------------");
            Console.WriteLine($"| User PIN code: {pinCode} |");
            Console.WriteLine("-----------------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> Please remember the PIN code. When you are ready to proceed press <Enter>...");
            Console.ReadLine();
        }
        public static void TransactionResult(bool result)
        {
            if (result)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-----------------------");
                Console.Write("| Successful Transfer |\n");
                Console.WriteLine("-----------------------");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("-------------------");
                Console.Write("| Failed Transfer |\n");
                Console.WriteLine("-------------------");
                Console.ResetColor();
            }
        }
        public static void BalanceRequest(int balance)
        {
            string userBalance = "User's balance: " + balance;
            int boxWidth = userBalance.Length + 4;
            string topBottomLine = new string('-', boxWidth);

            Console.WriteLine($"{topBottomLine}");
            Console.Write($"| User's balance: ");

            if (balance <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(balance);
            }
            else if (balance > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(balance);
            }

            Console.ResetColor();
            Console.Write(" |\n");
            Console.WriteLine($"{topBottomLine}");
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }
        public static string GetClientOption()
        {
            Console.Clear();
            Console.Write("Please choose one of the following:\n\n");
            Console.WriteLine("----------------------");
            Console.WriteLine("[1] Create account");
            Console.WriteLine("[2] Request PIN change");
            Console.WriteLine("[3] Revoke certificate");
            Console.WriteLine("----------------------");
            Console.WriteLine("[4] Deposit");
            Console.WriteLine("[5] Withdraw");
            Console.WriteLine("[6] Check Balance");
            Console.WriteLine("----------------------");
            Console.WriteLine(" |");
            Console.Write(" --> ");
            return Console.ReadLine();
        }
        public static void PrintCommunicationAndOption(Communication communication, Option option)
        {
            Console.Clear();

            switch (communication)
            {
                case Communication.WCF:
                    Console.WriteLine("----------------------");
                    Console.WriteLine("| Communication: WCF |");
                    Console.WriteLine("----------------------");
                    break;

                case Communication.AUTH:
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("| Communication: Windows Authentification |");
                    Console.WriteLine("-------------------------------------------");
                    break;

                case Communication.CERT:
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("| Communication: Certificates |");
                    Console.WriteLine("-------------------------------");

                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("| Invalid communication type |");
                    Console.WriteLine("------------------------------");
                    Console.ResetColor();

                    break;
            }
            switch (option)
            {
                case Option.CreateAccount:
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Option: Create Account");
                    break;

                case Option.RevokeCertificate:
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Option: Revoke Certificate");
                    break;

                case Option.ResetPIN:
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Option: Reset PIN");
                    break;

                case Option.Deposit:
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Option: Deposit");
                    break;

                case Option.Withdraw:
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Option: Withdraw");
                    break;

                case Option.CheckBalance:
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Option: Check Balance");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" |");
                    Console.WriteLine(" --> Invalid option");
                    Console.ResetColor();
                    break;
            }
        }
        public static void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("---------");
            Console.WriteLine("| Error |");
            Console.WriteLine("---------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }
        public static void PrintWrongPin()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-------------");
            Console.WriteLine("| Wrong pin |");
            Console.WriteLine("-------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }
        public static void PrintCorrectPin()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("----------------");
            Console.WriteLine("| Corrrect pin |");
            Console.WriteLine("----------------");
            Console.ResetColor();
        }

        public static void PrintPinChanged()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("----------------");
            Console.WriteLine("| Pin changed. |");
            Console.WriteLine("----------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();

        }
        public static void PrintRevokeCertificate()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------------");
            Console.Write("| Certificate successfully revoked.|\n");
            Console.WriteLine("------------------------------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }
        public static void PrintGenerateCertificate()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------------------------------------");
            Console.Write("| Certificate successfully generated.|\n");
            Console.WriteLine("--------------------------------------");
            Console.ResetColor();
        }
        public static void PrintCertInfo(X509Certificate2 cert)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------------------");
            Console.Write("| Certificate information:\t\t|\n");
            Console.Write($"| Subject name: {cert.SubjectName.Name}.\t\t|\n");
            Console.Write($"| Issuer: {cert.Issuer}.\t\t\t|\n");
            Console.WriteLine("-----------------------------------------");
            Console.ResetColor();
        }

        public static void PrintCertNotFound()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------------------");
            Console.Write("| Certificate information:\t\t|\n");
            Console.Write($"| Certificate not found.\t\t|\n");
            Console.WriteLine("-----------------------------------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }
        
        public static void SetConsoleTitle()
        {
            Console.Title = $"Client started by: {Formatter.ParseName(WindowsIdentity.GetCurrent().Name)}";
        }

        public static void PrintCertNotRevoked()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------");
            Console.Write("| Certificate is not revoked. |\n");
            Console.WriteLine("-------------------------------");
            Console.ResetColor();
        }

        public static void PrintCertRevoked()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------");
            Console.Write("| Certificate is revoked. |\n");
            Console.Write("| You need a new account. |\n");
            Console.WriteLine("---------------------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }

        public static void PrintEnterAmount(string typeOfTr) // withdraw or deposit
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"| Enter the amount you wish to {typeOfTr} |");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine(" |");
            Console.Write(" --> ");
        }
        public static void PrintInvalidInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------");
            Console.WriteLine($"| Invalid input. |");
            Console.WriteLine($"| Try again.     |");
            Console.WriteLine("------------------");
            Console.ResetColor();
            Console.WriteLine(" |");
            Console.WriteLine(" --> When you are ready to proceed press <Enter>...");
            Console.ReadKey();
        }
    }
}
