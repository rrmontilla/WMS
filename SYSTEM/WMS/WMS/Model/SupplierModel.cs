using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Model
{
    public class SupplierModel
    {
        public int ID { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierCodeTag { get; set; }
        public string SupplierName { get; set; }
        public string businessAddress { get; set; }
        public string Tin { get; set; }
        public string CellNumber { get; set; }
        public string TelNumber { get; set; }
        public string ContactPerson { get; set; }
        public string ProductAvailed { get; set; }
        public string PT { get; set; }
        public string Country { get; set; }
        public string SupplierCurrency { get; set; }
    }
}
