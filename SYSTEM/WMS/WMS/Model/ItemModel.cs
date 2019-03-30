using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Model
{
    public class ItemModel
    {
        public int ID { get; set; }
        public string itemCode { get; set; }
        public string itemCodeTag { get; set; }
        public string itemName { get; set; }
        public string description { get; set; }
        public string brand { get; set; }
        public string unit { get; set; }
        public string supplierName { get; set; }
        public string ssLevel { get; set; }
        public string LTDelivery { get; set; }
        public string Inventory { get; set; }
    }
}
