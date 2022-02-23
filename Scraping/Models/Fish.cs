using System;
using System.Collections.Generic;

#nullable disable

namespace Scraping.Models
{
    public partial class Fish
    {
        public Fish()
        {
            Registros = new HashSet<Registro>();
        }

        public int Id { get; set; }
        public string FishName { get; set; }

        public virtual ICollection<Registro> Registros { get; set; }
    }
}
