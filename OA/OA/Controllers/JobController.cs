using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class JobController : Controller
    {
        public ActionResult JobManage()
        {
            return View();
        }

        OSMSEntities osms = new OSMSEntities();

        // GET: Job
        public ActionResult Select(int limit,int page,string Name)
        {
            

            var list = osms.Job.ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                list = osms.Job.Where(x=>x.Name.Contains(Name)).ToList();
            }
            
            var l = list.OrderByDescending(x=>x.CreateTime).Skip((page-1)*limit).Take(limit).ToList();

            var json = new {
                code = "0",
                msg="",
                count = list.Count(),
                data= l
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult isdel(Job j)
        {
            Job job = osms.Job.Where(x => x.Id == j.Id).FirstOrDefault();
            UpdateModel(job);
            int a = osms.SaveChanges();
            return Content("");
        }

        public ActionResult Operate(Guid? strId, string strName, string strCode)
        {
            ViewBag.strId = "";
            ViewBag.strName = "";
            ViewBag.strCode = "";

            if (strId != null)
            {
                ViewBag.strId = strId;
            }
            if (strName != null)
            {
                ViewBag.strName = strName;
            }
            if (strCode != null)
            {
                ViewBag.strCode = strCode;
            }
            
            return View();
        }

        public ActionResult Save(Job j)
        {
            int a;
            string state;
            if (j.Id != Guid.Empty)
            {
                state = "EditOK";
                Job job = osms.Job.Where(x => x.Id == j.Id).FirstOrDefault();
                osms.Entry<Job>(job).State = System.Data.Entity.EntityState.Modified;
                job.Code = j.Code;
                job.UpdateTime = DateTime.Now.ToLocalTime();;
                job.Name = j.Name;
                a = osms.SaveChanges();
            }
            else
            {
                state = "AddOK";
                j.Id = Guid.NewGuid();
                j.CreateTime = DateTime.Now.ToLocalTime();
                j.UpdateTime = DateTime.Now.ToLocalTime();
                j.IsDel = true;
                osms.Job.Add(j);
                 a = osms.SaveChanges();
            }

            if (a > 0)
            {
                return Content(state);
            }
            else
            {
                return Content("NO");
            }

        }

    }
}