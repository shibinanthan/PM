using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Common;

namespace WebConsole.Interfaces
{
    public interface IProjectGateway
    {
        bool ValidateUser(PMUser user);
    }
}
