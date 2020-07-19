using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class OrganizationStructureController : Controller
    {
        OSMSEntities osms = new OSMSEntities();

        // GET: OrganizationStructure
        public ActionResult OrganizationStructureManage()
        {
            return View();
        }

        public ActionResult select()
        {
            //var list = osms.OrganizationStructure.Where(x => x.IsDel != true).ToList();
            var list = (from a in osms.OrganizationStructure
                    where a.IsDel != true
                    select new
                    {
                        Name = a.Name,
                        Id = a.Id,
                        ParentId = a.ParentId == null ? "0" : a.ParentId.ToString(),
                        Leve = a.Leve,
                        Code = a.Code
                    }).ToList();
            var json = new
            {
                code = "0",
                msg = "",
                count = list.Count(),
                data=list,

            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult del(Guid strId)
        {
            OrganizationStructure or = osms.OrganizationStructure.Where(x => x.Id == strId).FirstOrDefault();
            osms.Entry<OrganizationStructure>(or).State = System.Data.Entity.EntityState.Modified;
            or.IsDel = true;
            int a = osms.SaveChanges();
            if (a > 0)
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
            
            
        }

        public ActionResult Operate(Guid ? strid, string state)
        {
            organizationStructureViewData data = new organizationStructureViewData();
            if (state == "edit")
            {
                data = (from a in osms.OrganizationStructure
                        join b in osms.OrganizationStructure
                        on a.ParentId equals b.Id
                        into db
                        from b in db.DefaultIfEmpty()
                        where a.Id == strid
                        select new organizationStructureViewData
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Code = a.Code,
                            Leve = a.Leve,
                            ParentId = b.Id,
                            ParentName = b.Name == null ? "" : b.Name,
                            ParentCode = b.Code == null ? "" : b.Code
                        }).FirstOrDefault();
            }
            else if (state == "add")
            {
                data = (from a in osms.OrganizationStructure
                        where a.Id == strid
                        select new organizationStructureViewData
                        {
                            Name = "",
                            Code = "",
                            Leve = a.Leve + 1,
                            ParentId = a.Id,
                            ParentName = a.Name,
                            ParentCode = a.Code
                        }).FirstOrDefault();

            }
            
            ViewBag.data = data;
            return View();
        }

        public ActionResult Save(OrganizationStructure organizationStructure)
        {
            int a;
            string msg = string.Empty;
            if (organizationStructure.Id == Guid.Empty)
            {
                msg = "AddOk";
                organizationStructure.Id = Guid.NewGuid();
                organizationStructure.CreateTime = DateTime.Now.ToLocalTime();
                osms.OrganizationStructure.Add(organizationStructure);
                a = osms.SaveChanges();
            }
            else
            {
                msg = "EditOk";
                OrganizationStructure or = osms.OrganizationStructure.Where(x => x.Id == organizationStructure.Id).FirstOrDefault();
                osms.Entry<OrganizationStructure>(or).State = System.Data.Entity.EntityState.Modified;
                or.UpdateTime= DateTime.Now.ToLocalTime();
                or.Code = organizationStructure.Code;
                or.Name = organizationStructure.Name;
                a = osms.SaveChanges();
            }
            
            if (a>0)
            {
                return Content(msg);
            }
            else
            {

                return Content("NO");
            }
        }

    }
}