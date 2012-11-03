using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using docbox.Models;

namespace docbox.Utilities
{
    public class DbCommonQueries
    {

        public static List<string> getDepartmentName(string userId, dx_docboxEntities database)
        {
            List<string> departmentNames=new List<string>();

            if (database != null)
            {
                var departmentsIds = from userDepts in database.DX_USERDEPT where userDepts.userid == userId.Trim() select userDepts.deptid;

                if (departmentsIds.ToList().Count > 0)
                {

                    var depts = from departments in database.DX_DEPARTMENT where departmentsIds.Contains(departments.deptid) select departments.name;
                    if (depts.ToList().Count > 0)
                        departmentNames =(List<string>) (depts.ToList());
                }
            }
            return departmentNames;
        }


    }
}