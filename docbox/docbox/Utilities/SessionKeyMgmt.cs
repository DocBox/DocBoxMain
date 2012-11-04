﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace docbox.Utilities
{
    /// <summary>
    /// This is class to set session values for a user
    /// Using decorater pattern as will help with typecasting and not puting and getting values from wrong keys
    /// </summary>
    public static class SessionKeyMgmt
    {
        //private const String userAuthValue="permissions";// Decide later
        private const string userEmail = "emailAddress";
        private const string userDept = "userDepartment";

        public static string UserId
        {
            get
            {
                string userId =(string) HttpContext.Current.Session[userEmail];
                if (userId == null)
                {
                    return "";
                }
                return userId;

            }

             set
            {
                HttpContext.Current.Session[userEmail] = value;
            }

        }

        public static List<string> UserDept
        {
            get
            {
                List<string> userDepartment = (List<string>)HttpContext.Current.Session[userDept];
                if (userDepartment == null)
                {
                    return new List<string>();
                }
                return userDepartment;

            }

             set
            {
                HttpContext.Current.Session[userDept] = value;
            }

        }




    }
}