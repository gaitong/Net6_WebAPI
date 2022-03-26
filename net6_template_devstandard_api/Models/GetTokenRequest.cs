using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net6_template_devstandard_api.Models
{
    public class GetTokenRequest
    {
        public string EmpId { get; set; }
        public string Password { get; set; }
    }
}
