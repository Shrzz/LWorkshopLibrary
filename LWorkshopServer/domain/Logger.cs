using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer.domain
{
    public static class Logger
    {
        private static bool isInitialized;

        public static BindingList<string> Log { get; private set; }
        
        public static void Initialize()
        {
            if (isInitialized)
                return;

            Log = new BindingList<string>();
        }
    }
}
