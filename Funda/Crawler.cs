using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Funda
{
    public class Crawler
    {
        public Crawler()
        {
            this.Driver = new ChromeDriver(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Driver\"));

        }

        public void Navigate(Search search)
        {
            this.Driver.Url = search.Url;
            this.Driver.Navigate();
        }

        public List<Rent> Process()
        {
            var list = new List<Rent>();
            var adds = this.Driver.FindElementsByCssSelector(".search-result");
            foreach (var advert in adds)
            {
                list.Add(this.GetRent(advert));
            }

            return list;
        }

        public class Search
        {
            public int? MinRooms { get; set; }
            public int? MaxRooms { get; set; }
            public int? PriceMin { get; set; }
            public int? PriceMax { get; set; }
            public string Sorting { get { return "sorteer-datum-af/"; } }
            public string Text { get; set; }
            public string SaleOrRent { get; set; }
            public int? PaginationNumber { get; set; }

            public string Url
            {
                get
                {
                    string url = "http://www.funda.nl/";
                    url += SaleOrRent == "sale" ? "koop" : "huur";
                    url += "/";
                    if (this.MinRooms.HasValue || this.MaxRooms.HasValue)
                    {
                        var maxString = !this.MaxRooms.HasValue ? "+kamers" : string.Format("-{0}-kamers", this.MaxRooms.Value);
                        if (!this.MinRooms.HasValue) { this.MinRooms = 0; }
                        url += this.MinRooms.ToString();
                        url += maxString;
                        url += "/";
                    }

                  //  return "http://www.funda.nl/huur/rotterdam/1-4-kamers/sorteer-datum-af/p2/";
                    url += this.Sorting;
                    return url;
                }
            }
        }

        public object GetSale()
        {
            throw new NotImplementedException();
        }

        public Rent GetRent(IWebElement element)
        {
            var url = element.FindElements(By.CssSelector("a"));
            var title = element.FindElement(By.CssSelector(".search-result-title"));
            var subTitle = element.FindElement(By.CssSelector(".search-result-subtitle"));
            var price = element.FindElement(By.CssSelector(".search-result-price"));
            var livingArea = element.FindElement(By.CssSelector("[title='Woonoppervlakte']"));
            var totalArea = element.FindElement(By.CssSelector("[title='Perceeloppervlakte']"));
            var roomCount = livingArea.FindElement(By.XPath(".."));
            var parsedPrice = 0M;
            var parsedLivingArea = 0;
            var parsedTotalArea = 0;
            var parsedRoomCount = 0;
            return new Rent
            {
                Url = url[2].GetAttribute("href"),
                Title = title.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[0],
                Subtitle = subTitle.Text,
                Price = decimal.TryParse(new Regex("([0-9]*)").Matches(price.Text)[2].Value, out parsedPrice) ? parsedPrice : (decimal?)null,
                LivingArea = int.TryParse(new System.Text.RegularExpressions.Regex("([0-9]*)").Matches(livingArea.Text)[0].Value, out parsedLivingArea) ? parsedLivingArea : (int?)null,
                TotalArea = int.TryParse(new System.Text.RegularExpressions.Regex("([0-9]*)").Matches(totalArea.Text)[0].Value, out parsedTotalArea) ? parsedTotalArea : (int?)null,
                RoomCount = int.TryParse(roomCount.Text.Split(new string[] { "\r\n", "\n", "•", "kamers" }, StringSplitOptions.None)[1].Trim(), out parsedRoomCount) ? parsedRoomCount : (int?)null,
                Address = title.Text.Replace("\r\n", ""),
                PostCode = new System.Text.RegularExpressions.Regex("([1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2})", RegexOptions.IgnoreCase).Matches(subTitle.Text)[0].Value
            };
        }

        private ChromeDriver Driver { get; set; }
    }
}
