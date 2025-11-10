using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AccountDA
    {
        public List<Account> GetAll()
        {
            List<Account> list = new List<Account>();
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.Account_GetAll, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Account account = new Account();
                    account.AccountName = reader["AccountName"].ToString();
                    account.Password = reader["Password"].ToString();
                    account.FullName = reader["FullName"].ToString();
                    account.Email = reader["Email"]?.ToString();
                    account.Tell = reader["Tell"]?.ToString();
                    account.DateCreated = reader["DateCreated"] as DateTime?;
                    list.Add(account);
                }
            }
            return list;
        }

        public Account GetByID(string accountName)
        {
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.Account_GetByID, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountName", accountName);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Account account = new Account();
                    account.AccountName = reader["AccountName"].ToString();
                    account.Password = reader["Password"].ToString();
                    account.FullName = reader["FullName"].ToString();
                    account.Email = reader["Email"]?.ToString();
                    account.Tell = reader["Tell"]?.ToString();
                    account.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                    return account;
                }
            }
            return null;
        }

        public int Insert(Account account)
        {
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.Account_Insert, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountName", account.AccountName);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@FullName", account.FullName);
                cmd.Parameters.AddWithValue("@Email", account.Email ?? "");
                cmd.Parameters.AddWithValue("@Tell", account.Tell ?? "");
                cmd.Parameters.AddWithValue("@DateCreated", account.DateCreated);

                return cmd.ExecuteNonQuery();
            }
        }

        public int Update(Account account)
        {
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.Account_Update, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountName", account.AccountName);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@FullName", account.FullName);
                cmd.Parameters.AddWithValue("@Email", account.Email ?? "");
                cmd.Parameters.AddWithValue("@Tell", account.Tell ?? "");

                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(string accountName)
        {
            using (SqlConnection conn = new SqlConnection(Utilities.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Utilities.Account_Delete, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountName", accountName);
                return cmd.ExecuteNonQuery();
            }
        }


    }
}
