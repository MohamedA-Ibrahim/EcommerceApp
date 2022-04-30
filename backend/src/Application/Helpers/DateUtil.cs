using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class DateUtil
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.UtcNow.AddHours(2);
        }
    }
}
