using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    public class ResponseLogin
    {
        public bool Status { get; set;  }
        public string Token { get; set; }
        public TimeSpan Expirate { get; set; }
        public object StatusCode { get; internal set; }
        public IEnumerable<Login> Data { get; internal set; }
    }
}
