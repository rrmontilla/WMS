using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Model
{
    public class UserModel
    {
        public int ID { get; set; }
        public string FName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int position { get; set; }
        public int Department { get; set; }
        public int Branch { get; set; }
        public string Signature { get; set; }
    }
}
