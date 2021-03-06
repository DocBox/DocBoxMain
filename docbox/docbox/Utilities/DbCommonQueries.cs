﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using docbox.Models;


namespace docbox.Utilities
{
    public class DbCommonQueries
    {
        public static dx_docboxEntities db = new dx_docboxEntities();
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


        public static bool FileExistence(Int64 FileID)
        {
            var fileIdList = from filetable in db.DX_FILES select filetable;
            fileIdList.ToList();
            bool flag = false;
            foreach (DX_FILES file in fileIdList)
            {
                if (file.fileid == FileID)
                {
                    flag = true;
                }
            }
            if (flag == false)
                return false;
            else
                return true;
        }

       

        public static List<int> getDepartmentIds(string userId, dx_docboxEntities database)
        {
            List<int> departmentIdsUser = new List<int>();

            if (database != null)
            {
                var departmentsIds = from userDepts in database.DX_USERDEPT where userDepts.userid == userId.Trim() select userDepts.deptid;

                if (departmentsIds.ToList().Count > 0)
                {

                        departmentIdsUser = (List<int>)(departmentsIds.ToList());
                }
            }
            return departmentIdsUser;
        }

        public static int getDepartmentId(string deptname, dx_docboxEntities database)
        {
            if (database != null && deptname != null)
            {
                try
                {
                    var deptid = database.DX_DEPARTMENT.SingleOrDefault(department => department.name == deptname).deptid;

                    if (deptid > 0)
                    {
                        return Convert.ToInt32(deptid);
                    }
                }
                catch (Exception)
                {

                }
            }
            return 0;
        }
    }
}