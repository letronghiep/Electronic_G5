using System.Web.Mvc;

namespace Electronic_G5.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional }, // Controller và Action
           new[] { "Electronic_G5.Areas.Admin.Controllers" }
            );
        }
    }
}