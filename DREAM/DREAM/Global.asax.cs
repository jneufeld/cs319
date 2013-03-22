using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DREAM.Models;

namespace DREAM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Role.CreateRoles();

            var context = new DREAMContext();
            var initializeDomain = new CreateDatabaseIfNotExists<DREAMContext>();
            var initializeMigrations = new MigrateDatabaseToLatestVersion<DREAMContext, DREAM.Migrations.Configuration>();

            //initializeDomain.InitializeDatabase(context);
            //initializeMigrations.InitializeDatabase(context);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DREAMContext, DREAM.Migrations.Configuration>());

#if DEBUG && FALSE
            Database.SetInitializer(new DREAM.Models.DREAMContextInitializer());
#endif
        }
    }
}
