using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class ApproverController : Controller
    {
        OSMSEntities osms = new OSMSEntities();

        // GET: Approver
        public ActionResult ApproverManage()
        {
            ViewBag.Proc = osms.ProcessItem.ToList();

            return View();
        }

        public ActionResult selectApprover(int limit, int page,Guid strId)
        {
            var list = (from a in osms.Approver where a.ProcessItemId == strId select new { a.Id,a.JobCode,a.Discrible,a.Order,a.ProcessItemId}).OrderBy(x => x.Order).ToList();
            var json = new
            {
                code = "0",
                msg = "",
                count = list.Count(),
                data = list
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult upApprover(Guid strid)
        {
            Approver a = osms.Approver.Where(x => x.Id == strid).FirstOrDefault();
            Approver b = osms.Approver.Where(x => x.ProcessItemId == a.ProcessItemId & x.Order == (a.Order - 1)).FirstOrDefault();

            osms.Entry(a).State = System.Data.Entity.EntityState.Modified;
            osms.Entry(b).State = System.Data.Entity.EntityState.Modified;

            a.Order = a.Order - 1;
            b.Order = b.Order + 1;

            int l = osms.SaveChanges();

            if (l>0)
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }

        public ActionResult downApprover(Guid strid)
        {
            Approver a = osms.Approver.Where(x => x.Id == strid).FirstOrDefault();
            Approver b = osms.Approver.Where(x => x.ProcessItemId == a.ProcessItemId & x.Order == (a.Order + 1)).FirstOrDefault();

            osms.Entry(a).State = System.Data.Entity.EntityState.Modified;
            osms.Entry(b).State = System.Data.Entity.EntityState.Modified;

            a.Order = a.Order + 1;
            b.Order = b.Order - 1;

            int l = osms.SaveChanges();

            if (l > 0)
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }


    }
}