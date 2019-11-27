using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Domain.Entities;

namespace Domain.Abstract
{
   public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        User GetUserByID(int id);

        User GetUserByName(string login);

       void CreateUser(string login, string password, string email, bool isActivated, string passwordSalt, int userRoleId);

        bool ValidateUser(string login, string password);

        void SaveUser(User user);

        string GetUserNameByEmail(string email); //----???????

        MembershipUser GetMembershipUserByName(string login);

        IQueryable<User> UsersInfo { get; }

        void DeleteUser(User user);

        bool ActivateUser(string username, string key);

        //
        bool AdminExists();

        void CreateRole(string roleName);

        //roles
        void AddUserToRole(string username, string roleName);

        bool IsUserInRole(string username, string roleName);

    //    void RemoveUserFromRole(string username, string roleName);

        string[] GetRolesForUser(string username);

        IQueryable<Role> Roles { get; }

       // IQueryable<UserRole> UserRoles { get; }

        void DeleteRole(Role role);

        void SaveRole(Role role);
    }
}
