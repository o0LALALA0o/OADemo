using OA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    public class ProcessStepRecordController : Controller
    {
        OSMSEntities osms = new OSMSEntities();

        // GET: ProcessStepRecord
        public ActionResult select(Guid ? strRefOrderId)
        {
            var list = (from a in osms.ProcessStepRecord join b in osms.Staff on a.WaitForExecutionStaffId equals b.Id join c in osms.Job on b.JobId equals c.Id join d in osms.OrganizationStructure on b.OrgID equals d.Id where a.RefOrderId == strRefOrderId orderby a.StepOrder select new { Result=a.Result, WhetherToExecute=a.WhetherToExecute,uName=b.Name,JobName=c.Name, OrgName=d.Name }).ToList();

            var j = new {
                code = "0",
                msg = "",
                count = list.Count,
                data = list

            };

            return Json(j,JsonRequestBehavior.AllowGet);
        }

        public ActionResult start(OilMaterialOrder oi, Guid? p_id)
        {
            //var list = (from a in osms.ProcessStepRecord join b in osms.Staff on a.WaitForExecutionStaffId equals b.Id join c in osms.Job on b.JobId equals c.Id join d in osms.OrganizationStructure on b.OrgID equals d.Id where a.RefOrderId == strRefOrderId orderby a.StepOrder select new { Result = a.Result, WhetherToExecute = a.WhetherToExecute, uName = b.Name, JobName = c.Name, OrgName = d.Name }).ToList();
            var ap = osms.Approver.Where(x => x.ProcessItemId == p_id).ToList();

            Staff sta = osms.Staff.Where(x => x.Id == oi.ApplyPersonId).FirstOrDefault();

            //获取机构
            List<Guid?> listOrg = new List<Guid?>();
            listOrg.Add(sta.OrgID);
            selorg(sta.OrgID,ref listOrg);

            foreach (var item in ap)
            {
                var s = (from a in osms.Staff
                           join b in osms.Job
                           on a.JobId equals b.Id
                           where (listOrg.Contains(a.OrgID)) && b.Code == item.JobCode
                           select new
                           {
                               WaitForExecutionStaffId = a.Id
                           }).FirstOrDefault();

                ProcessStepRecord psr = new ProcessStepRecord();
                psr.Id = Guid.NewGuid();
                psr.StepOrder = item.Order;
                psr.CreateTime = DateTime.Now.ToLocalTime();
                psr.UpdateTime = DateTime.Now.ToLocalTime();
                psr.WaitForExecutionStaffId = s.WaitForExecutionStaffId;
                psr.WhetherToExecute = false;
                psr.No = oi.No;
                psr.RefOrderId = oi.Id;
                psr.Result = false;
                psr.Type = (osms.ProcessItem.Where(x=>x.Id==p_id).FirstOrDefault()).Code;
                psr.Discrible = "未审批";

                osms.ProcessStepRecord.Add(psr);

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

        //获取机构，递归
        public List<Guid?> selorg (Guid? oid,ref List<Guid?> listOrg)
        {
            OrganizationStructure l = osms.OrganizationStructure.Where(x => x.Id == oid).FirstOrDefault();
            if (l.ParentId != null)
            {
                listOrg.Add(l.ParentId);
                selorg(l.ParentId,ref listOrg);
            }
            
                return listOrg;
            
        }
    }
}