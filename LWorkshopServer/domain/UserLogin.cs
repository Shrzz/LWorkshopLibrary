using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer.domain
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsLibraryian { get; set; }
        public User User { get; set; }

        public bool canLogin(string login, string password)
        {
            return Login == login && Password == password ? true : false;
        }
    }
}
