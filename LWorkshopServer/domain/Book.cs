using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LWorkshopServer
{
    public class Book
    {
        public virtual int Id { set; get; }
        public virtual string Name { set; get; }
        public virtual string Author { set; get; }
        public virtual DateTime PublishingDate { set; get; }
        public virtual int NumOfBooks { set; get; }

    }
}
