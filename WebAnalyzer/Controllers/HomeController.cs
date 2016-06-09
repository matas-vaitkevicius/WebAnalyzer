using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var crawler = new Funda.Crawler();

            for (int i = 0; i < 10; i++)
                {
                var search = new Funda.Crawler.Search { SaleOrRent = "sale", MaxRooms = 3, MinRooms = 1, PaginationNumber = i, PriceMax = 125000 };
                crawler.Navigate(search);
              var results =  crawler.Process(search);
            }

            return View();
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