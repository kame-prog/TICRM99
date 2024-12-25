using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TICRM.CRMFilters
{
    public class BaseActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
           
        }
    }
}