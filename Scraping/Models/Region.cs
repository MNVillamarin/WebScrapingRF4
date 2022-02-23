using System;
using System.Collections.Generic;

#nullable disable

namespace Scraping.Models
{
    public partial class Region
    {
        public Region()
        {
            Registros = new HashSet<Registro>();
        }

        public int Id { get; set; }
        public string RegionName { get; set; }
        public string RegionUrl { get; set; }

        public virtual ICollection<Registro> Registros { get; set; }
    }
}
