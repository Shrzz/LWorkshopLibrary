using System;

namespace LWorkshopServer
{
    public class Issuance
    {
        public int Id { set; get; }
        public DateTime IssuanceDate { set; get; }
        public DateTime Deadline { set; get; }
        public virtual Book Book { set; get; }
        public virtual User User { set; get; }

        public bool IsDebt()
        {
            if (Deadline < DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetDebtDays()
        {
            return Deadline < DateTime.Now ? (DateTime.Now - Deadline).Days : 0;
        }
    }
}
