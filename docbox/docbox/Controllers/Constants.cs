using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace docbox.Controllers
{
    public class Constants
    {

       
        public static readonly String ADMIN_USER_ACCESS = "admin";

        public static readonly String TEMP_USER_ACCESS = "temp";
        public static readonly String EMPLOYEE_USER_ACCESS = "employee";
        public static readonly String VP_USER_ACCESS = "vp";
        public static readonly String MANAGER_USER_ACCESS = "manager";
        public static readonly String CEO_USER_ACCESS = "ceo";
        public static readonly String GUEST_USER_ACCESS = "guest";
        public static readonly String ADMINLESS_USER_ACCESS = "adminless";
        

     
        public static String POSITION_EMPLOYEE_USER= "employee";
        public static String POSITION_VP_USER = "vp";
        public static String POSITION_MANAGER_USER = "manager";
        public static String POSITION_CEO_USER = "ceo";
        



        public static Dictionary<int, string> secrateQuestionList = new Dictionary<int, string>()
    {
        {1,"What is name of your favorite childhood friend?"},
        {2,"What was the last name of your favorite teacher in primary school?"},
        {3,"In what city or town did your father and mother meet"},
        {4,"Waht is your maternal grandmother's maiden name?"},
        {5,"What was name of your first stuffed animal?"},
        {6,"What street did you lived in 1st grade?"},
        {7,"Which is your favorite Airport?"},
        {8,"What is the name of a University you applied to but didn't attend?"},
        {9,"In what city and country do you want to retire?"},

    };

    }
}