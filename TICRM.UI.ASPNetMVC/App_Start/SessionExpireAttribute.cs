using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TICRM.UI.ASPNetMVC.App_Start
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            // Check if session has expired
            if (session != null && session.IsNewSession)
            {
                string cookieHeader = filterContext.HttpContext.Request.Headers["Cookie"];
                if (cookieHeader != null && cookieHeader.IndexOf("ASP.NET_SessionId") >= 0)
                {
                    // Redirect to a session expired page or perform any other desired action
                    //filterContext.Result = new RedirectResult("~/Account/Login");
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "SessionExpire" // Assuming "Login" is the name of your view
                    };
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}