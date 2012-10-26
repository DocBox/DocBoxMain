using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Models;

namespace docbox.Filters
{
    public class CustomRoleAttribute: AuthorizeAttribute, IDisposable
    {
        dx_docboxEntities database = new dx_docboxEntities();
      
       

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized=false;
            if (!httpContext.Request.IsAuthenticated)
                return false;

            var currentUser = httpContext.User.Identity.Name;
             string[] roles;
            var userInof = database.DX_USER.FirstOrDefault(x => x.userid == currentUser);
            if (userInof == null)
            {
                roles=null;
            }
            else
            {
                roles = new string[1];
                roles[0] = userInof.accesslevel;
               
            }

            foreach (string eachRole in this.Roles.Split(','))
            {
                foreach (string role in roles)
                {

                    if (eachRole.Equals(role))
                    {
                        isAuthorized = true;
                        break;

                    }

                }

                if (isAuthorized)
                    break;
            }
            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/");
            base.HandleUnauthorizedRequest(filterContext);
        }




        public void Dispose()
        {
            database.Dispose();
        }
    }
   
}