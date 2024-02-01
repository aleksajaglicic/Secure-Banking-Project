using System.ServiceModel;

namespace Common.Interface
{
    [ServiceContract]
    public interface IWindowsAuthManagement
    {
        [OperationContract]
        void RevokeCertificate(string username, string pin);

        [OperationContract]
        byte[] DataBackup();

        [OperationContract]
        string RevokedCertsBackup();

        [OperationContract]
        bool CheckUsersPin(string username, string pinCode);

        [OperationContract]
        void test();
    }
}
