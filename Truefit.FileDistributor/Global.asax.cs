using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Truefit.FileDistributor
{
    public class MvcApplication : HttpApplication
    {
        public static string MetadataDirectory {
            get { return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MetaDirectory"]); }
        }

        public static string FileDirectory
        {
            get { return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FileDirectory"]); }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*alljs}", new { alljs = @".*\.js(/.*)?" });
            routes.IgnoreRoute("{*allhtm}", new { allhtm = @".*\.htm(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Upload",
                "Upload",
                new { controller = "Upload", action = "Index"}
            );

            routes.MapRoute(
                "Default", // Route name
                "Download/{id}", // URL with parameters
                new { controller = "Download", action = "Index", id = "" } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            CreateDirectory(MetadataDirectory);
            CreateDirectory(FileDirectory);
        }

        private static void CreateDirectory(string directory) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
        }
    }
}