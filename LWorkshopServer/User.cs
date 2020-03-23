using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace LWorkshopServer
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Issuance> Issuances { get; set; }
    }
}
