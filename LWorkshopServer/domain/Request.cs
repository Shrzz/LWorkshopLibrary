using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer.domain
{
    class Request
    {
        public string Query
        {
            get;
            set;
        }

        public UserLogin Login
        {
            get;
            set;
        }
    }
}
