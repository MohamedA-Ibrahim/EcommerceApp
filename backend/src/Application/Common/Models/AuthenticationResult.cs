using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string UserId { get;set;}
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
