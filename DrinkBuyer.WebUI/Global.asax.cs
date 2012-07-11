using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DrinkBuyer.Domain.Concrete;
using DrinkBuyer.Domain.Entities;
using DrinkBuyer.WebUI.Binders;

namespace DrinkBuyer.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("MyRoute",
                    "{controller}/{action}/{category}/{subcategory}",
                    // Matches /Product/SubCategory/Category/SubCategory
                    new {controller = "Product", action = "SubCategory", id = UrlParameter.Optional});

            /*routes.MapRoute(
                null,
                "products/{category}/{subCategory}",
                // Matches /Natural/InABottle
                new {controller = "Product", action = "SubCategory"})
                ;*/

            routes.MapRoute(
                null,
                "",
                // Only matches the empty URL (i.e. /)
                new {controller = "Product", action = "List", category = (string) null, page = 1});

            routes.MapRoute(
                null,
                "Page{page}",
                // Matches /Page2, /Page123, but not /PageXYZ
                new {controller = "Product", action = "List", category = (string) null},
                new {page = @"\d+"} // Constraints: page must be numerical
                );

            routes.MapRoute(
                null,
                "{category}",
                //Matches /Juice or /AnythingWithNoSlash
                new {controller = "Product", action = "List", page = 1});

            routes.MapRoute(
                null,
                "{category}/Page{page}",
                // Matches /Juice/Page567
                new {controller = "Product", action = "List"},
                // Defaults
                new {page = @"\d+"} // Constraints: page must be numerical
                );


            routes.MapRoute(null, "{controller}/{action}");
        }

        protected void Application_Start()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFDbContext>());
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof (Cart), new CartModelBinder());
        }
    }
}