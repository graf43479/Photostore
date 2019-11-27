using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            //---------------------------------------------------
            routes.MapRoute("Activate", "Account/Activate/{username}/{key}", new { controller = "Account", action = "Activate", username = UrlParameter.Optional, key = UrlParameter.Optional });
            //---------------------------------------------------
            routes.MapRoute("Robots.txt", "robots.txt", new { controller = "Home", action = "Robots" });
            //---------------------------------------------------
            routes.MapRoute("Sitemap.xml", "sitemap.xml", new { controller = "Home", action = "Sitemap" });
            //---------------------------------------------------
            routes.MapRoute(null, "Contact", new { controller = "Home", action = "Contact" }); //contacts
            //----------------------------------------------------
            routes.MapRoute(null, "Comments", new { controller = "Home", action = "Comments" });
            //----------------------------------------------------
            routes.MapRoute(null, "About", new { controller = "Home", action = "About" });
            //----------------------------------------------------
            routes.MapRoute(null, "Price", new { controller = "Home", action = "Price" });
            //----------------------------------------------------
            routes.MapRoute(null, "Portfolio", new { controller = "Home", action = "Portfolio" });
            //----------------------------------------------------
            routes.MapRoute(null, "Calendar", new { controller = "Home", action = "Calendar" }); 
            //----------------------------------------------------
            routes.MapRoute(null, "Admin", new { controller = "Admin", action = "Index" });
            //----------------------------------------------------
            routes.MapRoute(null, "Account", new { controller = "Home", action = "Login" }); //contacts
            //----------------------------------------------------
            //routes.MapRoute(null, "", new { controller = "Home", action = "Portfolio", category = (string)null, page = 1 });
            //----------------------------------------------------
            routes.MapRoute(null, "Page{page}", new { controller = "Home", action = "Portfolio", category = (string)null }, new { page = @"\d+" });
            //----------------------------------------------------
            routes.MapRoute("Category", "{category}", new { controller = "Home", action = "Portfolio", page = 1 });
            //----------------------------------------------------
            routes.MapRoute(null, "{category}/Page{page}", new { controller = "Home", action = "Portfolio" }, new { page = @"\d+" });
            //----------------------------------------------------
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(null, "{category}", new { controller = "Product", action = "List", page = 1 });
          
        }
    }
}