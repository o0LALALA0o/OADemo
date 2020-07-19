using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            //Menu menu = new Menu();
            //Staff s = Session["userInfo"] as Staff;
            //if(s!=null)
            //    ViewBag.l = menu.getMenu(s.Id);
            return View();
        }

        OSMSEntities osms = new OSMSEntities();

        [ActionLoginFiler(IsCheck=false)]
        public ActionResult Login()
        {
            return View();
        }

        [ActionLoginFiler(IsCheck = false)]
        public ActionResult selectLogin(string LoginName, string Loginpwd)
        {
            string pwd = EncryptHelper.Encode(Loginpwd);

            Staff l = osms.Staff.Where(x => (x.Email == LoginName & x.Password == pwd) || (x.No == LoginName & x.Password == pwd) || (x.Tel == LoginName & x.Password == pwd)).FirstOrDefault();
            if (l != null)
            {
                Session["userInfo"] = l;
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }


    }
}