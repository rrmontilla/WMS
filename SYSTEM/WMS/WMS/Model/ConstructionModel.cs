using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Model
{
    public class ConstructionModel
    {
        public string GroupCode { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
