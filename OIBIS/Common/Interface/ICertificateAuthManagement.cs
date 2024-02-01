using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface
{
    [ServiceContract]
    public interface ICertificateAuthManagement
    {
        [OperationContract]
        bool ResetPIN(string username, string oldPinCode, string newPinCode);

        [OperationContract]
        bool Deposit(string username, string pinCode, string value, byte[] signature);

        [OperationContract]
        bool Withdraw(string username, string pinCode, string value, byte[] signature);

        [OperationContract]
        int CheckBalance(string username, string pinCode);

        [OperationContract]
        bool IsPinValid(string username, string pinCode);

        [OperationContract]
        bool IsCertRevoked(string username, string pinCode);

        [OperationContract]
        void test();
    }
}
