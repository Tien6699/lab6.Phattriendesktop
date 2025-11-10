using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class RoleBL
    {
        RoleDA roleDA = new RoleDA();

        public List<Role> GetAllRoles()
        {
            return roleDA.GetAllRoles();
        }

        public List<RoleAccount> GetUserRoles(string accountName)
        {
            return roleDA.GetUserRoles(accountName);
        }

        public int UpdateUserRoles(string accountName, List<RoleAccount> roles)
        {
            return roleDA.UpdateUserRoles(accountName, roles);
        }

        public bool HasPermission(string accountName, string roleName)
        {
            var userRoles = GetUserRoles(accountName);
            var allRoles = GetAllRoles();

            foreach (var userRole in userRoles)
            {
                var role = allRoles.Find(r => r.ID == userRole.RoleID);
                if (role != null && role.RoleName == roleName && userRole.Actived)
                    return true;
            }
            return false;
        }

        public List<string> GetUserRoleNames(string accountName)
        {
            List<string> roleNames = new List<string>();
            var userRoles = GetUserRoles(accountName);
            var allRoles = GetAllRoles();

            foreach (var userRole in userRoles)
            {
                if (userRole.Actived)
                {
                    var role = allRoles.Find(r => r.ID == userRole.RoleID);
                    if (role != null)
                        roleNames.Add(role.RoleName);
                }
            }
            return roleNames;
        }
    }
}
