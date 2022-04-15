using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class UserAcc
    {
        public int UserId{ get; set; }

        public string  UserName { get; set; }
        public string UserFullName { get; set; }

        public string  UserPassword { get; set; }

        public bool IsActive { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }
    }
}
