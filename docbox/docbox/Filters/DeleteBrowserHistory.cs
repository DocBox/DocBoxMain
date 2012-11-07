using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace docbox.Filters
{
    public class DeleteBrowserHistoryAttribute:ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext usersContext)
        {
            usersContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            usersContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            usersContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            usersContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            usersContext.HttpContext.Response.Cache.SetNoStore();
         
        }
    }

    
}