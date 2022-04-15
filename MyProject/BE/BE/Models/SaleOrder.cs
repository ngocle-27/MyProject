using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class SaleOrder
    {
        public int SaleOrderId { get; set; }
        public string OrderType{ get; set; }
        public string TransportMethod { get; set; }
        public string DeliverCode { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string DescriptionOrd { get; set; }
        public string StatusOrder { get; set; }
        public int CustomerId { get; set; }
        public int AreaId { get; set; }
        public int ItemId { get; set; }
        public int PriceId { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string AreaName { get; set; }

        public string UserName { get; set; }
        public string ItemName { get; set; }
        // tên thuế (VAT10, VAT8)
        //public string VatCode { get; set; }
        ////2 cái này fix trong bảng giá
        //public int VatPercent { get; set; }

        public decimal Total { get; set; }


        /* public Item Item { get; set; }
         public Customer Customer { get; set; }
         public Area Area { get; set; }
         public Price Price { get; set; }
         public UserAcc UserAcc { get; set; }*/

    }
    
}
