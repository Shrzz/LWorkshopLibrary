using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer
{
    public class Book
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Author { set; get; }
        public DateTime PublishingDate { set; get; }
        public int NumOfBooks { set; get; }
    }
}
