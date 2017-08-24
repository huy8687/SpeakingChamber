using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber.Extension
{
    public static class ObjectExtensions
    {
        private static string LogFile = "_log.txt";

#if DEBUG
        private static bool AllowLog = true;
#else
        private static bool AllowLog = false;
#endif

        public static void Log(this object obj, string content = null, [CallerMemberName] string name = null)
        {
            if (AllowLog)
            {
                var type = obj?.GetType();
                File.AppendAllText(LogFile,$"{DateTime.Now.ToString()}\t{content}\t{name}\t{type}\t\r\n");
            }
        }
    }
}
