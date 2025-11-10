using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccess
{
    public class Utilities
    {
        private static string StrName = "ConnectionStringName";
        public static string ConnectionString = ConfigurationManager.ConnectionStrings[StrName].ConnectionString;
        public static string Food_GetAll = "Food_GetAll";
        public static string Food_InsertUpdateDelete = "Food_InsertUpdateDelete";
        public static string Category_GetAll = "Category_GetAll";
        public static string Category_InsertUpdateDelete = "Category_InsertUpdateDelete";

        // Account
        public static string Account_GetAll = "Account_GetAll";
        public static string Account_GetByID = "Account_GetByID";
        public static string Account_Insert = "Account_Insert";
        public static string Account_Update = "Account_Update";
        public static string Account_Delete = "Account_Delete";

        // Role
        public static string Role_GetAll = "Role_GetAll";
        public static string RoleAccount_GetByUser = "RoleAccount_GetByUser";
        public static string RoleAccount_UpdateUserRoles = "RoleAccount_UpdateUserRoles";
        public static string RoleAccount_Insert = "RoleAccount_Insert";


    }
}
