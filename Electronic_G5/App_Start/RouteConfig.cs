using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Electronic_G5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Electronic_G5.Controllers" } // Định rõ namespace cho Controller trong không gian tên mặc định
                );

            routes.MapRoute(
              name: "Admin",
              url: "admin/{controller}/{action}/{id}",
              defaults: new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "Electronic_G5.Areas.Admin.Controllers" } // Định rõ namespace cho Controller bên ngoài Area
          );
        }
    }
}
