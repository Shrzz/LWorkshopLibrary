using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LWorkshopServer.domain;
using System.ComponentModel.DataAnnotations;

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

        [ConcurrencyCheck]
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

        [ConcurrencyCheck]
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
        [ConcurrencyCheck]
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
