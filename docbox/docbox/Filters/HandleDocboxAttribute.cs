using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Text;

namespace docbox.Filters
{
    public class HandleDocboxAttribute : HandleErrorAttribute
    {
        private ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                        
        public override void OnException(ExceptionContext filterContext)
        {
            try
            {
                string userId = filterContext.HttpContext.User.Identity.Name.ToString();
                DateTime dateTime = filterContext.HttpContext.Timestamp;
                string routeId = string.Empty;

                StringBuilder auditMessage = new StringBuilder();
                auditMessage.Append("UserName=").Append(userId + " | ");

                auditMessage.Append("TimeStamp=").Append(dateTime.ToString() + " | ");

                if (filterContext.Exception != null)
                {
                    log.Error("Error : " + auditMessage.ToString(), filterContext.Exception);
                }

                base.OnException(filterContext);
            }
            catch (Exception e)
            {
            }
        }
    }
}