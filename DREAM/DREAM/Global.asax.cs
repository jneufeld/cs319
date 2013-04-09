using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lucene.Net.Store;
using DREAM.Models;
using DREAM.Migrations;

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

            var context = new DREAMContext();
            var initializeDomain = new CreateDatabaseIfNotExists<DREAMContext>();
            var initializeMigrations = new MigrateDatabaseToLatestVersion<DREAMContext, Configuration>();

            //initializeDomain.InitializeDatabase(context);
            //initializeMigrations.InitializeDatabase(context);

            //using (FSDirectory d = FSDirectory.Open(new DirectoryInfo(SearchIndex.DirPath)))
            //{
            //    SearchAutoComplete sac = new SearchAutoComplete(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/SearchAutocompleteIndex");
            //    sac.BuildAutoCompleteIndex(d, "Keywords");
            //}

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DREAMContext, Configuration>());

#if DEBUG && FALSE
            Database.SetInitializer(new DREAM.Models.DREAMContextInitializer());
#endif
        }
    }
}
