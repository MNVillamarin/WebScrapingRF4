using System;
using System.Collections.Generic;

#nullable disable

namespace Scraping.Models
{
    public partial class Location
    {
        public Location()
        {
            Registros = new HashSet<Registro>();
        }

        public int Id { get; set; }
        public string LocationName { get; set; }

        public virtual ICollection<Registro> Registros { get; set; }
    }
}
