using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class UserCustomer
    {
        public int UerCustomerId { get; set; }

        public int UserId { get; set; }

        public string CustomerId { get; set; }

        public bool IsActive { get; set; }
    }
}
