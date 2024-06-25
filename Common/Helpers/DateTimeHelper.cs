using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo customTimeZone = TimeZoneInfo.CreateCustomTimeZone("CustomTimeZone", TimeSpan.FromHours(7), "CustomTimeZone", "CustomTimeZone");

        public static DateTime VnNow
        {
            get
            {
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow, customTimeZone);
            }
        }
    }
}
