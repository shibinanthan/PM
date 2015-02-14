using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ProjectManagement.Common.Contracts
{
    [ServiceContract]
    interface IProjectService
    {
        [OperationContract(Name = "ValidateUser")]
        bool ValidateUser(PMUser user);
    }
}
