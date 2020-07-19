using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA.Models
{
    public class selectOilMaterialOrderViewModels:OilMaterialOrder
    {
        public string Name { get; set; }

        public Guid ApplyPersonId { get; set; }

        public string Discrible { get; set; }
    }
}