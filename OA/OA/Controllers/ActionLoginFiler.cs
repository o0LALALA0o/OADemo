using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class ActionLoginFiler: ActionFilterAttribute
    {
        public bool IsCheck { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsCheck)
            {
                if (filterContext.HttpContext.Session["userInfo"] == null)
                {
                    filterContext.HttpContext.Response.Redirect("/Main/Login");//重定向
                }
            }
        }

    }
}