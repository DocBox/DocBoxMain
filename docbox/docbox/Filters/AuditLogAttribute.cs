using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using docbox.Utilities;
using log4net;
using System.Reflection;

namespace docbox.Filters
{
    public class AuditLogAttribute : ActionFilterAttribute, IActionFilter
    {
       static readonly DocboxLoggingSerivce logService=new DocboxLogger(LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType));
       
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var MvcAction = filterContext.ActionDescriptor;

            string Controller = MvcAction.ControllerDescriptor.ControllerName;
            string Action = MvcAction.ActionName;
            string userId = filterContext.HttpContext.User.Identity.Name.ToString();
            DateTime dateTime = filterContext.HttpContext.Timestamp;
            string routeId = string.Empty;
            StringBuilder auditMessage = new StringBuilder();
            auditMessage.Append("UserName=").Append(userId + "|").Append("Controller=");
            auditMessage.Append(Controller + "|").Append("Action=").Append(Action + "|");
            auditMessage.Append("TimeStamp=").Append(dateTime.ToString() + "|");
            if (!string.IsNullOrEmpty(routeId))
            {
                auditMessage.Append("RouteId=");
                auditMessage.Append(routeId);
            }
            logService.Log(auditMessage.ToString());
            base.OnActionExecuted(filterContext);
        }
    }
}