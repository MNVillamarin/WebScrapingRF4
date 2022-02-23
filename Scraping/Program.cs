using System;
using System.Collections.Generic;
using System.Linq;
using Scraping.Aplication;
using Scraping.Models;

namespace Scraping
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (RF4RecordsContext db = new RF4RecordsContext())
            {
                List<Region> Regions = db.Regions.ToList();

                Scrap scrap = new Scrap(db);

                int records = scrap.Scraping(Regions);

                Console.WriteLine("Registros insertados: "+records);

            }
        }
    }
}
