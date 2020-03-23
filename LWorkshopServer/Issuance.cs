using System;

namespace LWorkshopServer
{
    public class Issuance
    {
        public int Id { set; get; }
        public DateTime IssuanceDate { set; get; }

        public virtual Book Book { set; get; }
        public virtual User User { set; get; }
    }
}
