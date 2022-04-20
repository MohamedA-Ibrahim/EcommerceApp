using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
