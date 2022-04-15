using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Area
    {
        public int AreaId { get; set; }

        public string AreaCode { get; set; }

        public string AreaName { get; set; }

        public bool IsActive { get; set; }
    }
}
