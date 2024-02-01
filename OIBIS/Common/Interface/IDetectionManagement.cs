using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface
{
    [ServiceContract]
    public interface IDetectionManagement
    {
        [OperationContract]
        void DetectWithdrawText(string username, string period, string numOfTransactions, string serviceName, string value);
    }
}
