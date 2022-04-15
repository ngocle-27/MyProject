using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public bool IsActive { get; set; }

      /*  public int SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }*/

    }
}
