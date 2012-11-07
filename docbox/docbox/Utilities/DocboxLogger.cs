using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Reflection;
using log4net.Core;

namespace docbox.Utilities
{
    public class DocboxLogger : DocboxLoggingSerivce
    {
        private  readonly ILog log;

        public DocboxLogger(ILog logService) 
        {
            this.log = logService;
        }
        public void Log(string message)
        {
            if(message!=null && !"".Equals(message))
            log.Info(message);
        }
    }

}