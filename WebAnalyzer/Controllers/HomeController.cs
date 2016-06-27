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
        private List<Crawler.Search> _aroudasSearches;
        public List<Funda.Crawler.Search> AruodasSearches
        {
            get
            {
                if (_aroudasSearches == null)
                {
                    _aroudasSearches = new List<Funda.Crawler.Search>
                    {
                        new Funda.Crawler.AruodasSearch { Text="butai/kaune/", FDistrict=6, FRegion=43, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="butu-nuoma/kaune/", FDistrict=6, FRegion=43, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="namai/kaune/", FDistrict=6, FRegion=43, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="namu-nuoma/kaune/", FDistrict=6, FRegion=43, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="butai/vilniuje/", FDistrict=1, FRegion=461, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="butu-nuoma/vilniuje/", FDistrict=1, FRegion=461, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="namai/vilniuje/", FDistrict=1, FRegion=461, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="namu-nuoma/vilniuje/", FDistrict=1, FRegion=461, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="butai/klaipedoje/", FDistrict=7, FRegion=112, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="butu-nuoma/klaipedoje/", FDistrict=7, FRegion=112, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="namai/klaipedoje/", FDistrict=7, FRegion=112, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="namu-nuoma/klaipedoje/", FDistrict=7, FRegion=112, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="butai/siauliuose/", FDistrict=11, FRegion=259, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="butu-nuoma/siauliuose/", FDistrict=11, FRegion=259, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="namai/siauliuose/", FDistrict=11, FRegion=259, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="namu-nuoma/siauliuose/", FDistrict=11, FRegion=259, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="butai/panevezyje/", FDistrict=10, FRegion=205, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="butu-nuoma/panevezyje/", FDistrict=10, FRegion=205, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                        new Funda.Crawler.AruodasSearch { Text="namai/panevezyje/", FDistrict=10, FRegion=205, PriceMin=4000, PriceMax= 50000, MinRooms = 1, MaxRooms = 4 },
                        new Funda.Crawler.AruodasSearch { Text="namu-nuoma/panevezyje/", FDistrict=10, FRegion=205, PriceMin=0, PriceMax=1000, MinRooms=1, MaxRooms =4 },
                    };
                }
                return _aroudasSearches; } }

        public List<Funda.Crawler.Search> Searches
        {
            get
            {
                if (_searches == null)
                {
                    _searches = new List<Funda.Crawler.Search>
                {
                   new Funda.Crawler.FundaSearch { Text="Rotterdam", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Rotterdam", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Amsterdam", PriceMin=70000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Amsterdam", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Eindhoven", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Eindhoven", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Utrecht", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Utrecht", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Tilburg", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Tilburg", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Leiden", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Leiden", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Breda", PriceMin=50000, PriceMax= 1300000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Breda", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Dordrecht", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Dordrecht", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="den-haag", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="den-haag", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Groningen", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Groningen", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Almere", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Almere", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Nijmegen", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Nijmegen", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Enschede", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Enschede", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Apeldoorn", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Apeldoorn", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Haarlem", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Haarlem", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Amersfoort", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Amersfoort", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Arnhem", PriceMin=50000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Arnhem", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Zwolle", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Zwolle", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="Zoetermeer", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="Zoetermeer", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="gemeente-zaanstad", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="gemeente-zaanstad", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="den-bosch", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="den-bosch", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                   new Funda.Crawler.FundaSearch { Text="gemeente-haarlemmermeer", PriceMin=45000, PriceMax= 130000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.FundaSearch { Text="gemeente-haarlemmermeer", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
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
                                adverts = crawler.AddNewSales().Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
                                db.Sale.AddRange(adverts.Cast<Sale>().ToList());
                            }
                            else
                            {
                                adverts = crawler.AddNewRents().Where(o => o.Price != null).ExceptWhere(db.Rent, o => o.Url);
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


        public ActionResult CollectNewLt()
        {
           using (var crawler = new Funda.Crawler())
            {
                using (var db = new Funda.WebAnalyzerEntities())
                {
                   foreach (var search in AruodasSearches)
                    {
                       // SetMinMax(search);
                       for (int i = 1; i < 15; i++)
                       {
                           search.PaginationNumber = i;
                           crawler.Navigate(search);
                           var adverts = Enumerable.Empty<IFundaRecord>();
                          if (search.IsSale)
                          {
                               adverts = crawler.AddNewLtSales().Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
                              db.Sale.AddRange(adverts.Cast<Sale>().ToList());
                          }
                          else
                           {
                               adverts = crawler.AddNewLtRents().Where(o => o.Price != null).ExceptWhere(db.Rent, o => o.Url);
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

           return RedirectToAction("UpdateExistingLt");
        }

        public ActionResult UpdateExistingLt(bool? isSale)
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
                            list = db.Rent.Where<IFundaRecord>(o => o.DateRemoved == null && o.Url.Contains("aruodas")).ToList().Union(db.Sale.Where<IFundaRecord>(o => o.DateRemoved == null && o.Url.Contains("aruodas")).ToList()).ToList();
                        }
                        else
                        {
                            if (isSale.Value)
                            {
                                list = db.Sale.Where<IFundaRecord>(o => o.DateRemoved == null && o.Url.Contains("aruodas")).ToList();
                            }
                            else
                            {
                                list = db.Rent.Where<IFundaRecord>(o => o.DateRemoved == null && o.Url.Contains("aruodas")).ToList();
                            }
                        }

                        foreach (var rent in list)
                        {
                            try
                            {
                                crawler.Navigate(rent.Url);
                                crawler.GetRecordDataFromItsPageLt(rent);
                                db.SaveChanges();

                            }
                            catch { }
                        }
                    }
                }
            }
            return View("Index");
        }

    }
}