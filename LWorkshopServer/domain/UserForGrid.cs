using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer.domain
{
    public class UserForGrid
    {
        public UserForGrid() { }
        public UserForGrid(User user)
        {
            User = user;
            Books = new List<Book>();
        }

        public int Id { get => User.Id; }
        public string Name { get => User.Name; }
        public virtual List<Book> Books { set; get; }
        public User User { get; set; }

        public bool HasDebts()
        {
            foreach (var i in User.Issuances)
            {
                if (i.IsDebt())
                {
                    return true;
                }
            }

            return false;
        }

        public List<Issuance> GetDebts()
        {
            List<Issuance> debts = new List<Issuance>();

            foreach (var i in User.Issuances)
            {
                if (i.Deadline < DateTime.Now)
                {
                    debts.Add(i);
                }
            }

            return debts;
        }

        public Issuance FindIssuance(Book b)
        {
            if (b == null)
                throw new Exception("Значение книги null");

            Issuance issuance = null;

            foreach (Issuance i in User.Issuances)
            {
                if (i.Book == b)
                {
                    issuance = i;
                    break;
                }
            }

            return issuance;
        }

        public static Issuance FindUserIssuance(User u, Book b)
        {
            if (b == null || u == null)
                throw new Exception("Значение книги null");

            Issuance issuance = null;

            foreach (Issuance i in u.Issuances)
            {
                if (i.Book == b)
                {
                    issuance = i;
                    break;
                }
            }

            return issuance;
        }

        //public double CountDebts()
        //{
        //    double tariff = new OptionsModel().Initialize().Tariff;
        //    List<Issuance> debts = GetDebts();
        //    double result = 0;

        //    foreach (var debt in debts)
        //    {
        //        result += debt.GetDebtDays() * tariff;
        //    }

        //    return result;
        //}
    }
}
