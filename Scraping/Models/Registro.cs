using System;
using System.Collections.Generic;

#nullable disable

namespace Scraping.Models
{
    public partial class Registro
    {
        public int Id { get; set; }
        public int? Region { get; set; }
        public int? Fish { get; set; }
        public string Weight { get; set; }
        public int? Location { get; set; }
        public string Lure { get; set; }
        public string Player { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Fish FishNavigation { get; set; }
        public virtual Location LocationNavigation { get; set; }
        public virtual Region RegionNavigation { get; set; }
    }
}
