using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }
        public bool IsActive { get; set; }
    }
}
