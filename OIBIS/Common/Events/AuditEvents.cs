using System;
using System.Reflection;
using System.Resources;

namespace Common.Events
{
    public enum AuditEventTypes
    {
        CreateAccountSuccess = 0,
        CreateAccountFailed = 1,
        DepositSuccess = 2,
        DepositFailed = 3,
        WithdrawSuccess = 4,
        WithdrawFailed = 5,
        ResetPinSuccess = 6,
        ResetPinFailed = 7,
        RevokeCertSuccess = 8,
        RevokeCertFailed = 9,
        DepositRequestSuccess = 10,
        DepositRequestFailed = 11,
        WithdrawRequestSuccess = 12,
        WithdrawRequestFailed = 13,
        WithdrawDetection = 14,
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    try
                    {
                        if (resourceManager == null)
                        {
                            resourceManager = new ResourceManager
                                (typeof(Common.Events.AuditEventFile).ToString(),
                                Assembly.GetExecutingAssembly());
                        }
                        return resourceManager;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                return resourceManager;
            }
        }

        public static string CreateAccountSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateAccountSuccess.ToString());
            }
        }
        public static string CreateAccountFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateAccountFailed.ToString());
            }
        }
        public static string DepositSuccess
        {                    
            get              
            {                
                return ResourceMgr.GetString(AuditEventTypes.DepositSuccess.ToString());
            }                
        }                    
        public static string DepositFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DepositFailed.ToString());
            }
        }
        public static string WithdrawSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WithdrawSuccess.ToString());
            }
        }
        public static string WithdrawFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WithdrawFailed.ToString());
            }
        }
        public static string ResetPinSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ResetPinSuccess.ToString());
            }
        }
        public static string ResetPinFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ResetPinFailed.ToString());
            }
        }
        public static string RevokeCertSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RevokeCertSuccess.ToString());
            }
        }
        public static string RevokeCertFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RevokeCertFailed.ToString());
            }
        }
        public static string DepositRequestSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DepositRequestSuccess.ToString());
            }
        }
        public static string DepositRequestFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DepositRequestFailed.ToString());
            }
        }
        public static string WithdrawRequestSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WithdrawRequestSuccess.ToString());
            }
        }
        public static string WithdrawRequestFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WithdrawRequestFailed.ToString());
            }
        }
        public static string WithdrawDetection
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WithdrawDetection.ToString());
            }
        }
    }
}
