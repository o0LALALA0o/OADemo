using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace OA.Controllers
{
    public class OilMaterialOrderController : Controller
    {
        OSMSEntities osms = new OSMSEntities();

        // GET: OilMaterialOrder
        public ActionResult Show()
        {
            return View();
        }

        public ActionResult Save(string str,string s)
        {
            List<OilMaterialOrderDetail> od = JsonConvert.DeserializeObject<List<OilMaterialOrderDetail>>(s);
            OilMaterialOrder oi = JsonConvert.DeserializeObject<OilMaterialOrder>(str);
            
            oi.No = "YLSQ" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
            oi.Id = Guid.NewGuid();
            oi.CreateTime = DateTime.Now.ToLocalTime();
            oi.ApplyPersonId = (Session["userInfo"] as Staff).Id;
            oi.IsDel = false;

            osms.OilMaterialOrder.Add(oi);

            foreach (var item in od)
            {
                item.Id = Guid.NewGuid();
                item.OrderId = oi.Id;
                item.CreateTime = oi.CreateTime;
                item.IsDel = false;

                osms.OilMaterialOrderDetail.Add(item);
            }

            int i = osms.SaveChanges();

            if (i>0)
            {
                return Content("AddOk");
            }
            else
            {
                return Content("NO");
            }
        }

        public ActionResult del(Guid ? strId)
        {
            OilMaterialOrder oi = osms.OilMaterialOrder.Where(x => x.Id == strId).FirstOrDefault();
            List<OilMaterialOrderDetail> od = osms.OilMaterialOrderDetail.Where(x => x.OrderId == strId).ToList();

            foreach (var item in od)
            {
                osms.Entry(item).State = System.Data.Entity.EntityState.Modified;
                item.IsDel = true;
            }

            osms.Entry(oi).State = System.Data.Entity.EntityState.Modified;
            oi.IsDel = true;

            List<ProcessStepRecord> lpr = (osms.ProcessStepRecord.Where(x => x.RefOrderId == strId)).ToList();

            foreach (var item in lpr)
            {
                osms.ProcessStepRecord.Remove(item);
            }

            int i = osms.SaveChanges();

            if (i > 0)
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
            
        }

        public ActionResult Select(int limit, int page)
        {
            Guid strid = (Session["userInfo"] as Staff).Id;
            var list = (from a in osms.OilMaterialOrder join b in osms.Staff on a.ApplyPersonId equals b.Id  where a.ApplyPersonId == strid && a.IsDel == false select new { Id=a.Id, ApplyDate=a.ApplyDate,NO=a.No, CreateTime =a.CreateTime, uId=b.Id, Name=b.Name }).ToList();


            List<selectOilMaterialOrderViewModels> sel = new List<selectOilMaterialOrderViewModels>();
            foreach (var item in list)
            {
                var l = (from a in osms.ProcessStepRecord where a.RefOrderId == item.Id && a.WhetherToExecute == true orderby a.UpdateTime descending select new { Discrible = a.Discrible }).FirstOrDefault();
                selectOilMaterialOrderViewModels s = new selectOilMaterialOrderViewModels();
                if (l!=null)
                {
                    s.Discrible = l.Discrible;
                }
                else
                {
                    s.Discrible = "未发起";
                }
                
                s.Name = item.Name;
                s.ApplyDate = item.ApplyDate;
                s.Id = item.Id;
                s.CreateTime = item.CreateTime;
                s.No = item.NO;
                s.ApplyPersonId = item.uId;

                sel.Add(s);
            } 

            var json = new
            {
                code = "0",
                msg = "",
                count = sel.Count,
                data = sel.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit)
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}