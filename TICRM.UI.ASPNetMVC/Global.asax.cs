using TICRM.UI.ASPNetMVC.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System;
using log4net.Config;
using System.IO;
using System.Web;

namespace TICRM.UI.ASPNetMVC
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/log4net.config")));
        }

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    // Perform any cleanup tasks (e.g., remove session data, delete temporary files)
        //    // Redirect the user to the login page
        //    Response.Redirect("~/Account/Login");
        //}


        //protected void Session_End(object sender, EventArgs e)
        //{
        //    // Clear any session specific data
        //    Session.Abandon();
        //    // Redirect the user to a login page or display a session timeout message
        //    Response.Redirect("~/Account/Login");
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    // Redirect to the login page
        //    Response.Redirect("~/Account/Login");
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    // Clear any session specific data
        //    Session.Abandon();
        //    // Redirect the user to the login page
        //    HttpContext.Current.Response.Redirect("~/Account/Login");
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    // Set the flag in the session
        //    Session["RedirectRequired"] = true;
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    // Check if a redirect is required
        //    if (Session["RedirectRequired"] != null && (bool)Session["RedirectRequired"])
        //    {
        //        // Clear the flag
        //        Session["RedirectRequired"] = false;

        //        // Redirect the user to the login page
        //        Response.Redirect("~/Account/Login");
        //    }
        //}

    }
}
