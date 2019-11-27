using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Domain.Abstract;
using Ninject;

namespace Domain
{
    public class PrimaryRoleProvider : RoleProvider
    {
        [Inject]
        public IUserRepository usersRepository { get; set; }

        public override bool IsUserInRole(string username, string roleName)
        {
            return usersRepository.IsUserInRole(username, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {

            try
            {
                return usersRepository.GetRolesForUser(username);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override void CreateRole(string roleName)
        {
            usersRepository.CreateRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var u in usernames)
            {
                foreach (var r in roleNames)
                {
                    usersRepository.AddUserToRole(u, r);
                }
            }
        }

        public void AddUsersToRoles(string username, string roleName)
        {
            usersRepository.AddUserToRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
            //foreach (var u in usernames)
            //{
            //    foreach (var r in roleNames)
            //    {
            //        usersRepository.RemoveUserFromRole(u, r);
            //    }
            //}
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}
