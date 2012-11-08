using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using docbox.Models;
using System.Web.Mvc;

namespace docbox.Utilities
{
    public class ApplicationRoleProvider: RoleProvider, IDisposable

    {
        dx_docboxEntities database = new dx_docboxEntities();
        public void Dispose()
        {
            database.Dispose();
        }

        
        public override string[] GetRolesForUser(string userid)
        {
               
                
                var userInfo = from users in database.DX_USER where users.userid== userid select  users;
                if (userInfo == null)
                {
                    return null;
                }
                else
                {
                    string[] roles = {""};
                    
                    if(userInfo.ToList().Count==1){
                        roles[0] = userInfo.ToList().First().role;
                    }
                    return roles;

                }
           
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}