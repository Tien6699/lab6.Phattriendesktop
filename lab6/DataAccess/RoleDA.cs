using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoleDA
    {
        public List<Role> GetAllRoles()
        {
            List<Role> list = new List<Role>();
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.Role_GetAll, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Role role = new Role();
                    role.ID = Convert.ToInt32(reader["ID"]);
                    role.RoleName = reader["RoleName"].ToString();
                    role.Path = reader["Path"]?.ToString();
                    role.Notes = reader["Notes"]?.ToString();
                    list.Add(role);
                }
            }
            return list;
        }

        public List<RoleAccount> GetUserRoles(string accountName)
        {
            List<RoleAccount> list = new List<RoleAccount>();
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.RoleAccount_GetByUser, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountName", accountName);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RoleAccount roleAcc = new RoleAccount();
                    roleAcc.RoleID = Convert.ToInt32(reader["RoleID"]);
                    roleAcc.AccountName = reader["AccountName"].ToString();
                    roleAcc.Actived = Convert.ToBoolean(reader["Actived"]);
                    roleAcc.Notes = reader["Notes"]?.ToString();
                    list.Add(roleAcc);
                }
            }
            return list;
        }

        public int UpdateUserRoles(string accountName, List<RoleAccount> roles)
        {
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand deleteCmd = new SqlCommand(Utilities.RoleAccount_UpdateUserRoles, conn, transaction);
                        deleteCmd.CommandType = CommandType.StoredProcedure;
                        deleteCmd.Parameters.AddWithValue("@AccountName", accountName);
                        deleteCmd.ExecuteNonQuery();

                        int count = 0;
                        foreach (var role in roles)
                        {
                            if (role.Actived)
                            {
                                SqlCommand insertCmd = new SqlCommand(Utilities.RoleAccount_Insert, conn, transaction);
                                insertCmd.CommandType = CommandType.StoredProcedure;
                                insertCmd.Parameters.AddWithValue("@RoleID", role.RoleID);
                                insertCmd.Parameters.AddWithValue("@AccountName", role.AccountName);
                                insertCmd.Parameters.AddWithValue("@Actived", role.Actived);
                                insertCmd.Parameters.AddWithValue("@Notes", role.Notes ?? "");
                                count += insertCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return count;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }


    }
}
