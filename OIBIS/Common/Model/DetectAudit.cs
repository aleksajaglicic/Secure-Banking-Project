using System;
using System.Diagnostics;
using Common.Events;

namespace Common.Model
{

    public class DetectAudit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Common.Model.DetectAudit";
        const string LogName = "MyDetAudit";

        static DetectAudit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void WithdrawDetection(string username, string value, string serviceName, string numOfTransactions, string period)
        {
            if (customLog != null)
            {
                string UserWithdrawDetection = AuditEvents.WithdrawDetection;
                // User andje withdrew [3,4,5] from pera, 10 times in 50 seconds.
                string message = String.Format(UserWithdrawDetection, username, value, serviceName, numOfTransactions, period);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WithdrawDetection));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
