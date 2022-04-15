using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Price
    {
        public int PriceId { get; set; }

        public string PriceCode { get; set; }

        public string PriceName { get; set; }

        public bool IsActive { get; set; }
        public int ItemId { get; set; }
        public string VatCode { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
