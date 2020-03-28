using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LWorkshopServer.domain;

namespace LWorkshopServer
{
    public class LibraryContext : DbContext
    {
        DbSet<Book> books;
        DbSet<User> users;
        DbSet<Issuance> issuances;
        DbSet<UserLogin> logins;

        public LibraryContext() : base("DBConnectionString")
        {
        }

        public DbSet<Book> Books
        {
            set
            {
                books = value;
            }
            get
            {
                return books;
            }
        }
        public DbSet<User> Users
        {
            set
            {
                users = value;
            }
            get
            {
                return users;
            }
        }
        public DbSet<Issuance> Issuances
        {
            set
            {
                issuances = value;
            }
            get
            {
                return issuances;
            }
        }
        public DbSet<UserLogin> Logins
        {
            get => logins;
            set => logins = value;
        }
    }
}
