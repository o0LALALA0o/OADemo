﻿using OA.Controllers;
using System.Web;
using System.Web.Mvc;

namespace OA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionLoginFiler() { IsCheck = true });
        }
    }
}
