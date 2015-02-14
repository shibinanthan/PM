using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Common
{
    class ProjectModel
    {
    }

    public class PMUser : PMBaseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
    }

    public class PMBaseModel
    {
        public string ConnectionString { get; set; }
    }
}
