using System;
using System.Collections.Generic;

#nullable disable

namespace Scraping.Models
{
    public partial class FishLocation
    {
        public int Id { get; set; }
        public int? IdLocation { get; set; }
        public int? IdFish { get; set; }

        public virtual Fish IdFishNavigation { get; set; }
        public virtual Location IdLocationNavigation { get; set; }
    }
}
