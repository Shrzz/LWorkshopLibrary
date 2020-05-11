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
        private static BindingList<string> log = new BindingList<string>();
        public static BindingList<string> Log 
        {
            get
            {
                lock (log)
                {
                    return log;
                }
                
            }
        }
        
    }
}
