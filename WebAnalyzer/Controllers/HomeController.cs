using Funda;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System;

namespace WebAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        private List<Crawler.Search> _searches;

        public List<Funda.Crawler.Search> Searches
        {
            get
            {
                if (_searches == null)
                {
                    _searches = new List<Funda.Crawler.Search>
                {
                   new Funda.Crawler.Search { Text="Rotterdam", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Rotterdam", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Amsterdam", PriceMin=70000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Amsterdam", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Eindhoven", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Eindhoven", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Utrecht", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Utrecht", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Tilburg", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Tilburg", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Leiden", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Leiden", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Breda", PriceMin=50000, PriceMax= 1300000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Breda", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Dordrecht", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Dordrecht", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="den-haag", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="den-haag", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Groningen", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Groningen", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Almere", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Almere", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Nijmegen", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Nijmegen", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Enschede", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Enschede", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Apeldoorn", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Apeldoorn", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Haarlem", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Haarlem", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Amersfoort", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Amersfoort", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Arnhem", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Arnhem", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Zwolle", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Zwolle", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="Zoetermeer", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Zoetermeer", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="gemeente-zaanstad", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="gemeente-zaanstad", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="den-bosch", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="den-bosch", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.Search { Text="gemeente-haarlemmermeer", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="gemeente-haarlemmermeer", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                    };

                }
                return _searches;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public Crawler.Search SetMinMax(Crawler.Search search)
        {
            if (search.IsSale)
            {
                search.PriceMin = 40000;
                search.PriceMax = 130000;
            }
            else
            {
                search.PriceMin = 200;
                search.PriceMax = 1300;
            }

            return search;
        }

        public ActionResult CollectNew()
        {
            using (var crawler = new Funda.Crawler())
            {
                using (var db = new Funda.WebAnalyzerEntities())
                {
                    foreach (var search in Searches)
                    {
                        SetMinMax(search);
                        for (int i = 0; i < 10; i++)
                        {
                            search.PaginationNumber = i;
                            crawler.Navigate(search);
                            var adverts = Enumerable.Empty<IFundaRecord>();
                            if (search.IsSale)
                            {
                                adverts = crawler.AddNewSales(search).Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
                                db.Sale.AddRange(adverts.Cast<Sale>().ToList());
                            }
                            else
                            {
                                adverts = crawler.AddNewRents(search).Where(o => o.Price != null).ExceptWhere(db.Rent, o => o.Url);
                                db.Rent.AddRange(adverts.Cast<Rent>().ToList());
                            }

                            if (!adverts.Any())
                            {
                                break;
                            }

                            db.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("UpdateExisting");
        }

        object _lock;

        public ActionResult UpdateExisting(bool? isSale)
        {

            using (var crawler = new Crawler())
            {
                _lock = new object();
                lock (_lock)
                {
                    using (var db = new Funda.WebAnalyzerEntities())
                    {
                        var now = DateTime.Now;
                        var list = new List<IFundaRecord>();
                        if (!isSale.HasValue)
                        {
                            list = db.Rent.Where<IFundaRecord>(o => o.DateRemoved == null).ToList().Union(db.Sale.Where<IFundaRecord>(o => o.DateRemoved == null).ToList()).ToList();
                        }
                        else
                        {
                            if (isSale.Value)
                            {
                                list = db.Sale.Where<IFundaRecord>(o => o.DateRemoved == null).ToList();
                            }
                            else
                            {
                                list = db.Rent.Where<IFundaRecord>(o => o.DateRemoved == null).ToList();
                            }
                        }
                    
                        foreach (var rent in list)
                        {
                            try
                            {
                                crawler.Navigate(rent.Url);
                                crawler.GetRecordDataFromItsPage(rent);
                                db.SaveChanges();

                            }
                            catch { }
                        }
                    }
                }
            }
                    return View("Index"); 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}