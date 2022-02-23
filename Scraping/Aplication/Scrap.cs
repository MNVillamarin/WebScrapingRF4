using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Scraping.Models;
using ScrapySharp.Extensions;

namespace Scraping.Aplication
{
    public class Scrap
    {
        private readonly HtmlWeb _oWeb = new HtmlWeb();
        private readonly RF4RecordsContext _dbContext;

        public Scrap(RF4RecordsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Scraping(List<Region> regions)
        {
            int result = 0;

            foreach (var region in regions)
            {
                HtmlDocument doc = _oWeb.Load(region.RegionUrl);
                var registros = ScrapingRegion(doc, region.Id);
                SaveOnDb(registros);
            }
            return result;
        }


        private void SaveOnDb(List<Registro> registros)
        {
            foreach (var reg in registros)
            {
                Console.WriteLine(reg.Region.ToString() + ", " + reg.Fish + ", " + reg.Location + ", " + reg.Lure + ", " +
                                  reg.Weight + ", " + reg.Player + ", " + reg.Fecha);
            }

            _dbContext.BulkInsert(registros, options =>
            {
                options.ColumnPrimaryKeyExpression = sender => new
                {
                    sender.Region, sender.Fish, sender.Weight, sender.Location,
                    sender.Lure, sender.Player, sender.Fecha
                };
                options.AllowDuplicateKeys = true;
                options.InsertIfNotExists = true;
                options.ForceOutputFromUnmodifiedRow = false;
            });
        }


        private List<Registro> ScrapingRegion(HtmlDocument doc, int region)
        {
            List<Registro> registros = new List<Registro>();

            foreach (var Nodo in doc.DocumentNode.CssSelect(".records_subtable.flex_table"))
            {
                var valida = Nodo.InnerHtml;

                if (!valida.Contains("text disable"))//CssSelect(".text.disable").First().InnerText != null)
                {
                    var Record = new Registro();

                    #region RegistroPadre
                    Record.Region = region;

                    var FishName = Nodo.CssSelect(".text").First().InnerText;

                    Record.Fish = (from f in _dbContext.Fish
                                   where f.FishName == FishName
                                   select f.Id).First();


                    Record.Lure = Nodo.CssSelect(".bait_icon").First().Attributes["title"].Value;

                    Record.Weight = Regex.Match(Nodo.CssSelect(".col.overflow.nowrap.weight")
                            .First().InnerText,
                        @"\d+").Value;

                    Record.Location = (from f in _dbContext.Locations
                                       where f.LocationName == Nodo.CssSelect(".col.overflow.nowrap.location").First().InnerText
                                       select f.Id).First();

                    Record.Player = Nodo.CssSelect(".col.overflow.nowrap.gamername").First().InnerText;

                    Record.Fecha = DateTime.Parse(Nodo.CssSelect(".col.overflow.nowrap.data").First().InnerText);


                    registros.Add(Record);
                    #endregion


                    #region RegistrosHijos
                    var oHijos = Nodo.ChildNodes;
                    var NodoHijos = oHijos[1].ChildNodes;

                    foreach (var Hijo in NodoHijos)
                    {
                        var RecordHijo = new Registro();

                        RecordHijo.Region = region;

                        RecordHijo.Fish = (from f in _dbContext.Fish
                                           where f.FishName == FishName
                                           select f.Id).First();

                        RecordHijo.Lure = Hijo.CssSelect(".bait_icon").First().Attributes["title"].Value;

                        RecordHijo.Weight = Regex.Match(Hijo.CssSelect(".col.overflow.nowrap.weight")
                                .First().InnerText,
                            @"\d+").Value;

                        RecordHijo.Location = (from f in _dbContext.Locations
                                               where f.LocationName == Hijo.CssSelect(".col.overflow.nowrap.location").First().InnerText
                                               select f.Id).First();

                        RecordHijo.Player = Hijo.CssSelect(".col.overflow.nowrap.gamername").First().InnerText;

                        RecordHijo.Fecha = DateTime.Parse(Hijo.CssSelect(".col.overflow.nowrap.data").First().InnerText);

                        registros.Add(RecordHijo);
                    }


                    #endregion
                }
            }

            return registros;
        }


    }
}
