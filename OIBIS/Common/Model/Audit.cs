using System;
using System.Diagnostics;
using Common.Events;

namespace Common.Model
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Common.Model.Audit";
        const string LogName = "MySecTest";

        static Audit()
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

        public static void CreateAccountSuccess(string username, string serviceName)
        {
            if (customLog != null)
            {
                string UserCreateAccountSuccess = AuditEvents.CreateAccountSuccess;
                string message = String.Format(UserCreateAccountSuccess, username);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CreateAccountSuccess));
            }
        }
        public static void CreateAccountFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserCreateAccountFailed = AuditEvents.CreateAccountFailed;
                string message = String.Format(UserCreateAccountFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CreateAccountFailed));
            }
        }
        public static void DepositSuccess(string username, string serviceName, string amount)
        {
            if (customLog != null)
            {
                string UserDepositSuccess = AuditEvents.DepositSuccess;
                string message = String.Format(UserDepositSuccess, username, amount);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DepositSuccess));
            }
        }
        public static void DepositFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserDepositFailed = AuditEvents.DepositFailed;
                string message = String.Format(UserDepositFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DepositFailed));
            }
        }
        public static void WithdrawSuccess(string username, string serviceName, string amount)
        {
            if (customLog != null)
            {
                string UserWithdrawSuccess = AuditEvents.WithdrawSuccess;
                string message = String.Format(UserWithdrawSuccess, username, amount);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WithdrawSuccess));
            }
        }
        public static void WithdrawFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserWithdrawFailed = AuditEvents.WithdrawFailed;
                string message = String.Format(UserWithdrawFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WithdrawFailed));
            }
        }
        public static void ResetPinSuccess(string username, string serviceName)
        {
            if (customLog != null)
            {
                string UserResetPinSuccess = AuditEvents.ResetPinSuccess;
                string message = String.Format(UserResetPinSuccess, username);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ResetPinSuccess));
            }
        }
        public static void ResetPinFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserResetPinFailed = AuditEvents.ResetPinFailed;
                string message = String.Format(UserResetPinFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ResetPinFailed));
            }
        }
        public static void RevokeCertSuccess(string username, string serviceName)
        {
            if (customLog != null)
            {
                string UserRevokeCertSucces = AuditEvents.RevokeCertSuccess;
                string message = String.Format(UserRevokeCertSucces, username);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.RevokeCertSuccess));
            }
        }
        public static void RevokeCertFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserRevokeCertFailed = AuditEvents.RevokeCertFailed;
                string message = String.Format(UserRevokeCertFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.RevokeCertFailed));
            }
        }
        public static void DepositRequestSuccess(string username, string serviceName)
        {
            if (customLog != null)
            {
                string UserDepositRequestSuccess = AuditEvents.DepositRequestSuccess;
                string message = String.Format(UserDepositRequestSuccess, username);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DepositRequestSuccess));
            }
        }
        public static void DepositRequestFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserDepositRequestFailed = AuditEvents.DepositRequestFailed;
                string message = String.Format(UserDepositRequestFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DepositRequestFailed));
            }
        }
        public static void WithdrawRequestSuccess(string username, string serviceName)
        {
            if (customLog != null)
            {
                string UserWithdrawRequestSuccess = AuditEvents.WithdrawRequestSuccess;
                string message = String.Format(UserWithdrawRequestSuccess, username);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WithdrawRequestSuccess));
            }
        }
        public static void WithdrawRequestFailed(string username, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string UserWithdrawRequestFailed = AuditEvents.WithdrawRequestFailed;
                string message = String.Format(UserWithdrawRequestFailed, username, serviceName, reason);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WithdrawRequestFailed));
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
