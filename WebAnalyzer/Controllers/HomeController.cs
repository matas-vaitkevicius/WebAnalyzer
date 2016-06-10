using Funda;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

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
                   new Funda.Crawler.Search { Text="Rotterdam", PriceMin=45000, PriceMax= 125000, MinRooms = 1, MaxRooms = 4, IsSale = true },
                   new Funda.Crawler.Search { Text="Rotterdam", PriceMin=0, PriceMax=1300, MinRooms=1, MaxRooms =4, IsSale = false },
                };

                }
                return _searches;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CollectNew()
        {
            var crawler = new Funda.Crawler();
            using (var db = new Funda.WebAnalyzerEntities())
            {
                foreach (var search in Searches)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        search.PaginationNumber = i;
                        crawler.Navigate(search);

                        if (search.IsSale)
                        {
                            var adverts = crawler.AddNewSales(search).Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);

                            db.Sale.AddRange(adverts);
                        }
                        else
                        {
                            var adverts = crawler.AddNewRents(search).Where(o => o.Price != null).ExceptWhere(db.Rent, o => o.Url);
                            db.Rent.AddRange(adverts);

                        }

                        db.SaveChanges();
                    }
                }
            }

            return View("Index");
        }

        public ActionResult UpdateExisting()
        {
            var crawler = new Crawler();
            using (var db = new Funda.WebAnalyzerEntities())
            {
                foreach (var rent in db.Rent.Where(o => o.DateRemoved == null))
                {
                    try
                    {
                        crawler.Navigate(rent.Url);
                        var dates = crawler.GetRecordDates();
                        rent.DateAdded = dates.Item1;
                        rent.DateRemoved = dates.Item2;

                        db.SaveChanges();
                    }
                    catch { }
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