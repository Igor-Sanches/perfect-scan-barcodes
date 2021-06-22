using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan_Mobile.Tools
{
    public class TimerUtils
    {
        private static readonly DateTime Time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Time).TotalMilliseconds;
        }
    }
}
