using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace docbox.Controllers
{
    public class Constants
    {
        public static String TEMP_USER = "temp";
        public static String EMPLOYEE_USER = "employee";
        public static String VP_USER = "vp";
        public static String MANAGER_USER = "manager";
        public static String CEO_USER = "ceo";
        public static String GUEST_USER = "guest";
        public static String ADMIN_USER = "admin";


        public static Dictionary<int, string> secrateQuestionList = new Dictionary<int, string>()
    {
        {1,"What is your father's middle name?"},
        {2,"What was the last name of your favorite teacher?"},
        {3,"What was your first pet's name?"},
        {4,"Where did you meet your spouse"},
        {5,"What was your favorite food as child?"},
    };

    }
}