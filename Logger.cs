using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFireStarter
{
    public static class Logger
    {
        public static void Log(string message)
        {
            //Console.WriteLine(string.Format("SharpFireStarter : {0:HH:mm:ss tt} : {1}", DateTime.Now.TimeOfDay, message));
            Console.WriteLine(string.Format("SharpFireStarter : {0}", message));
        }
    }
}
