using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite
{
    public class SessionExpireActionFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Session.GetInt32("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/Authentication/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetInt32("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/Authentication/Login");
                return;
            }
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Session.GetInt32("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/Authentication/Login");
                return;
            }
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            if (filterContext.HttpContext.Session.GetInt32("UserID") == null)
            {
                //filterContext.Result = new RedirectResult("~/Authentication/Login");
                return;
            }
            base.OnResultExecuted(filterContext);
        }
    }
}
