using System.Web.Mvc;
using System.Web.Routing;

namespace docbox
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "PublicFiles",
                "Documents/PublicFiles",
                new { controller = "Documents", action = "PublicFiles" }
            );

            routes.MapRoute(
                "MyDocs",
                "Documents/",
                new { controller = "Documents", action = "ListDocuments" }
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //Adding configuration for log4net for audit and exception logggig
            log4net.Config.XmlConfigurator.Configure();
           // var container = new UnityContainer();
           // container.RegisterType<DocboxLoggingSerivce,DocboxLogger>(new InjectionConstructor(LogManager.GetLogger
           // (MethodBase.GetCurrentMethod().DeclaringType)));           
            //DependencyResolver.SetResolver(new UnityServiceLocator(container));
           
        }
    }
}