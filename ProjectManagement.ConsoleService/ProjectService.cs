using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data;
using System.Data.SqlClient;
using ProjectManagement.Common;
using ProjectManagement.Common.Contracts;

namespace ProjectManagement.ConsoleService
{
    [Serializable]
    [ServiceBehavior(Name = "DataService", Namespace = "Productservice")]
    public class ProjectService : IProjectService
    {
        public bool ValidateUser(PMUser user)
        {
            var returnValue = 0;
            using (SqlConnection connection = new SqlConnection(user.ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("SP_ValidateUsers", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = user.UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.Password;

                    SqlParameter statusParam = new SqlParameter();
                    statusParam.ParameterName = "@Status";
                    statusParam.DbType = DbType.Int32;
                    statusParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(statusParam);

                    cmd.ExecuteNonQuery();
                    returnValue = Convert.ToInt32(cmd.Parameters["@Status"].Value);
                }
            }
            return returnValue == 0 ? false : true;
        }
    }
}
