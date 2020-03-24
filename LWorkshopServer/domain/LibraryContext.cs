using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LWorkshopServer
{
    class LibraryContext : DbContext
    {
        public LibraryContext() : base("DbConnection")
        {
        }
        public List<User> Users { get; set; }
        public List<Book> Books { get; set; }
    }
}
