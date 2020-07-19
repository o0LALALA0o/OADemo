using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OA.Models;

namespace OA.Controllers
{
    public class Menu
    {
        OSMSEntities osms = new OSMSEntities();

        public List<SystemResourceModule> getMenu(Guid guid)
        {
            string sql = "select * from [dbo].[SystemResourceModule] where Id in (select ResourceModuleId from [dbo].[RoleResourceModule] where RoleId in (SELECT [RoleId]  FROM [OSMS].[dbo].[StaffRole] where StaffId='" + guid + "')) and type ='0'";
            return osms.Database.SqlQuery<SystemResourceModule>(sql).ToList(); 
        }
    }
}