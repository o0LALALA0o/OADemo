using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult StaffRoleManage()
        {
            return View();
        }

        OSMSEntities osms = new OSMSEntities();

        public ActionResult selectStaffRole(int limit, int page, string Name)
        {
            var list = osms.Staff.OrderByDescending(x=>x.CreateTime).Skip((page - 1) * limit).Take(limit).ToList();
            var json = new
            {
                code = "0",
                msg = "",
                count = osms.Staff.Count(),
                data = list
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult editStaffRole(List<Role> r,Guid strid)
        {
            var a = osms.StaffRole.Where(x => x.StaffId == strid).ToList();
            if (a.Count()>0)
            {
                foreach (var item in a)
                {
                    osms.StaffRole.Remove(item);
                }
            }

            if (r != null)
            {
                foreach (var item in r)
                {
                    StaffRole sro = new StaffRole();
                    sro.id = Guid.NewGuid();
                    sro.RoleId = item.Id;
                    sro.StaffId = strid;
                    osms.StaffRole.Add(sro);                
                }
            }
            
            
            osms.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectRole(int limit, int page, Guid strId)
        {
            var sL = osms.StaffRole.Where(x => x.StaffId == strId).ToList();

            var list =from a in osms.Role
                      select new {
                          Id=a.Id,
                          Name =a.Name,
                          Code=a.Code,                          
                          LAY_CHECKED= false
                      };

            List<selectRoleViewModels> selectRole = new List<selectRoleViewModels>();

            foreach (var item in list)
            {
                selectRoleViewModels s = new selectRoleViewModels();
                s.Id = item.Id;
                s.Name = item.Name;
                s.Code = item.Code;
                s.LAY_CHECKED = sL.Any(x => x.RoleId == item.Id);
                selectRole.Add(s);
            }

            
            //List<Role> r=list.a
            var json = new
            {
                code = "0",
                msg = "",
                count = selectRole.Count(),
                data = selectRole
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
    }
}