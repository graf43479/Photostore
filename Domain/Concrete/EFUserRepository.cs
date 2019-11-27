using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFUserRepository  :  IUserRepository
    {
        private PhotoDBContext context;

        public EFUserRepository(PhotoDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users.Include("Role");
        }

        public User GetUserByID(int id)
        {
            return context.Users.FirstOrDefault(x => x.UserID == id);
        }

        public User GetUserByName(string login)
        {
            return context.Users.FirstOrDefault(x => x.Login == login);
        }



        public void CreateUser(string login, string password, string email, bool isActivated,
                               string passwordSalt, int userRoleId)
        {
            User user = new User
            {
                Login = login,
                Email = email,
                Created = DateTime.Now,
                IsActivated = isActivated,
                PasswordSalt = passwordSalt,
                Password = password,
                NewEmailKey = GenerateKey(),
                RoleID = userRoleId
            };

            SaveUser(user);
        }

        public bool ValidateUser(string login, string password)
        {
            User user = context.Users.FirstOrDefault(x => x.Login.TrimEnd() == login);
            if (user != null && user.Password.TrimEnd() == password)
                //if (user != null && user.Password.TrimEnd() == )
                return true;
            return false;
        }

        public void SaveUser(User user)
        {
            if (user.UserID == 0)
                context.Users.Add(user);

            else
                context.Entry(user).State = EntityState.Modified;

            context.SaveChanges();
        }

        public string GetUserNameByEmail(string email)
        {
            User user = context.Users.FirstOrDefault(x => x.Email == email);
            return user != null ? user.Login : "";
        }

        public MembershipUser GetMembershipUserByName(string login)
        {
            User user = context.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                return new MembershipUser(
                    "CustomMembershipProvider",
                    user.Login,
                    user.UserID,
                    user.Email,
                    "",
                    null,
                    true,
                    false,
                    user.Created,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now
                    );
            }
            return null;
        }

        public IQueryable<User> UsersInfo {
            get { return context.Users.Include("Role"); }
        }


        public void DeleteUser(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public bool ActivateUser(string username, string key)
        {
            try
            {
                User user = UsersInfo.FirstOrDefault(x => x.Login == username);
                if (user.NewEmailKey == key)
                {
                    user.IsActivated = true;
                    user.NewEmailKey = null;

                    context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool AdminExists()
        {
            try
            {
                var role = context.Roles.FirstOrDefault(x => x.RoleName.ToLower() == "admin");
                if (role != null)
                {
                    //var userRole = context.Users.FirstOrDefault(x => x.RoleID == role.RoleID);
                    var user = context.Users.Where(x => x.RoleID == role.RoleID && x.IsActivated);

                    if (user.Any())
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }

        public void CreateRole(string roleName)
        {
            var roleExists = context.Roles.FirstOrDefault(x => x.RoleName == roleName);
            if (roleExists == null)
            {
                Role role = new Role()
                {
                    RoleName = roleName
                };
                context.Roles.Add(role);
                context.SaveChanges();
            }
        }

        public void AddUserToRole(string username, string roleName)
        {
            User user = context.Users.FirstOrDefault(x => x.Login == username);
            Role role = context.Roles.FirstOrDefault(x => x.RoleName == roleName);
            //UserRole userRole = context.UserRoles.FirstOrDefault(x => x.UserID == user.UserID);

            try
            {
                if (user != null && role != null)
                {
                    user.RoleID = role.RoleID;
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            


            //if (userRole == null)
            //{
            //    UserRole ur = new UserRole();
            //    ur.RoleID = role.RoleID;
            //    ur.UserID = user.UserID;
            //    //   context.UserRoles.InsertOnSubmit(ur);
            //    context.UserRoles.Add(ur);
            //}
            //else
            //{
            //    userRole.RoleID = role.RoleID;
            //    context.Entry(userRole).State = EntityState.Modified;
            //}
            
        }

        public bool IsUserInRole(string username, string roleName)
        {
            var p = from u in context.Users.Where(x => x.Login == username)
                    join r in context.Roles.Where(x => x.RoleName == roleName) on u.RoleID equals r.RoleID
                    select u.UserID;
           

            if (p.Any())
            {
                return true;
            }
            return false;
        }

        //public void RemoveUserFromRole(string username, string roleName)
        //{
        //    var user = context.Users.FirstOrDefault(x => x.Login == username);
        //    var role = context.Roles.FirstOrDefault(x => x.RoleName == roleName);

        //    var userRole = context.UserRoles.FirstOrDefault(x => x.UserID == user.UserID && x.RoleID == role.RoleID);
            
        //    userRole.RoleID = 0;
        //    context.Entry(userRole).State = EntityState.Modified;
        //    context.SaveChanges();
        //}

        public string[] GetRolesForUser(string username)
        {
            try
            {
                IList<string> roleNames = new List<string>();

                //var user = context.Users.FirstOrDefault(x => x.Login == username);

                //if (user==null)
                //{
                //    return null;
                //}
                var userRoles = from u in context.Users.ToList()
                                join r in context.Roles.ToList() on u.RoleID equals r.RoleID
                                where u.Login == username
                                select r.RoleName;
                 //context.UserRoles.Where(x => x.UserID == user.UserID).ToList();

                if (userRoles.Any()!=true)
                {
                    return null;
                }
                else
                {
                    foreach (var r in userRoles)
                    {
                        roleNames.Add(r);
                    }                    
                    return roleNames.ToArray();
                    //foreach (var r in userRoles)
                    //{
                    //    roleNames.Add(r.RoleName);
                    //}                    
                }


                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IQueryable<Role> Roles {
            get { return context.Roles; } 
        }

        public void DeleteRole(Role role)
        {
            context.Roles.Remove(role);
            context.SaveChanges();
        }

        public void SaveRole(Role role)
        {
            if (role.RoleID == 0)
            {
                context.Roles.Add(role);
            }
            else
            {
                context.Entry(role).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        private static string GenerateKey()
        {
            Guid emailKey = Guid.NewGuid();

            return emailKey.ToString();
        }
    }
}
