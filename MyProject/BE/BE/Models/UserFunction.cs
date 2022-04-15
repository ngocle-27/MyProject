using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class UserFunction
    {
        public int UserFunctionId { get; set; }

        public int UserId { get; set; }

        public string FunctionId { get; set; }

        public bool IsActive { get; set; }
    }
}
