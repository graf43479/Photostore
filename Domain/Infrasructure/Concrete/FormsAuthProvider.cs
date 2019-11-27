using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Domain.Infrasructure.Abstract;

namespace Domain.Infrasructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authentificate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, true);
            }
            return result;

        }
    }
}