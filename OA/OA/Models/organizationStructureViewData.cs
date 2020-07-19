using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA.Models
{
    public class organizationStructureViewData: OrganizationStructure
    {
        public string ParentName { get; set; }
        public string ParentCode { get; set; }
    }
}