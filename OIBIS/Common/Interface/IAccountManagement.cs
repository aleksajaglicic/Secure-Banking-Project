using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common { 

    [ServiceContract]
    public interface IAccountManagement
    {
        [OperationContract]
        string CreateAccount(string username);
    }
}
