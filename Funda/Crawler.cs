using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Specialized;

namespace Funda
{
    public class Crawler :IDisposable
    {
        public Crawler()
        {
            this.Driver = new ChromeDriver(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Driver\"));
        }

        public void Navigate(Search search)
        {
            Navigate(search.Url);
        }

        public void Navigate(string url)
        {
            this.Driver.Url = url;
            this.Driver.Navigate();
        }

        public List<Sale> AddNewSales(Search search)
        {
                var list = new List<Sale>();
                var adds = this.Driver.FindElementsByCssSelector(".search-result");
                foreach (var advert in adds)
                {
                    list.Add(this.GetSale(advert));
                }

            return list;
        }

        public List<Rent> AddNewRents(Search search)
        {
            var list = new List<Rent>();
            var adds = this.Driver.FindElementsByCssSelector(".search-result");
            foreach (var advert in adds)
            {
                list.Add(this.GetRent(advert));
            }

            return list;
        }

        public Regex NumberRegex {
            get { return new Regex("([0-9.]*)"); }
        }

        public class Search
        {
            public int? MinRooms { get; set; }
            public int? MaxRooms { get; set; }
            public int? PriceMin { get; set; }
            public int? PriceMax { get; set; }
            public string Sorting { get { return "sorteer-datum-af/"; } }
            public string Text { get; set; }
            public bool IsSale { get; set; }
            public int? PaginationNumber { get; set; }

            public string Url
            {
                get
                {
                    string url = "http://www.funda.nl/";
                    url += IsSale ? "koop" : "huur";
                    url += "/";
                    url += this.Text + "/";
                    if (this.MinRooms.HasValue || this.MaxRooms.HasValue) // jei minRooms turi reiksme ARBA maxRooms turi reiksme tada varyt per koda kur tarp {}
                        {
                            var maxString = !this.MaxRooms.HasValue ? "+kamers" : string.Format("-{0}-kamers", this.MaxRooms.Value);
                            if (!this.MinRooms.HasValue) { this.MinRooms = 0; }
                            url += this.MinRooms.ToString();
                            url += maxString;
                            url += "/";
                        }

                    if (this.PriceMin.HasValue || this.PriceMax.HasValue)
                   {
                          var maxString = !this.PriceMax.HasValue ? "+" : string.Format("-{0}", this.PriceMax.Value);
                          if (!this.PriceMin.HasValue) { this.PriceMin = 0; }
                          url += this.PriceMin.ToString();
                          url += maxString;
                          url += "/";
                  }
                    if (this.PaginationNumber.HasValue)
                    {
                        url += string.Format("p{0}/", this.PaginationNumber.Value);
                    }
                     
                       

                  //s  return "http://www.funda.nl/huur/rotterdam/1-4-kamers/sorteer-datum-af/p2/";
                    url += this.Sorting;
                    return url;
                }
            }
        }

        public IFundaRecord GetRecordDataFromItsPage(IFundaRecord fundaRecord)
        {
            if (!fundaRecord.DateAdded.HasValue)
            {
                DateTime? dateAdded = null;
                var dateRegex = new Regex("([0-9]{1,2}) ([a-zA-Z]*) 2016");
                var dateAddedElement = this.Driver.FindElementsByCssSelector(".object-primary .object-kenmerken-body .object-kenmerken-list dd").FirstOrDefault(o => dateRegex.IsMatch(o.Text));

                if (dateAddedElement != null)
                {
                    System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("nl-NL");
                    dateAdded = DateTime.Parse(dateAddedElement.Text, cultureinfo);
                }
                else
                {
                    dateAdded = DateTime.Now;
                }

                fundaRecord.DateAdded = dateAdded;
            }

            if (!fundaRecord.DateRemoved.HasValue)
            {
                DateTime? dateRemoved;
                try
                {
                    var dateRemovedElement = this.Driver.FindElementByCssSelector(".label-transactie-voorbehoud");
                    dateRemoved = DateTime.Now;
                }
                catch
                {
                    try
                    {
                        var addNotFoundElement = this.Driver.FindElementByCssSelector(".icon-not-found-house-blueBrand");
                        dateRemoved = DateTime.Now;
                    }
                    catch
                    {
                        dateRemoved = null;
                    }
                }

                fundaRecord.DateRemoved = dateRemoved;
            }

            if (fundaRecord is Rent)
            {
                decimal initialCostToRentOut = 0m;
                try
                {
                    var initialCostToRentOutElement = this.Driver.FindElement(By.CssSelector(".object-header-services-costs"));
                    foreach (var match in this.NumberRegex.Matches(initialCostToRentOutElement.Text))
                    {
                        if (decimal.TryParse(((Match)match).Value, out initialCostToRentOut))
                        {
                            break;
                        }
                    }

                    ((Rent)fundaRecord).InitialCostToRentOut = initialCostToRentOut;
                }
                catch { }
            }

            fundaRecord.DateLastProcessed = DateTime.Now;
            // initialCostToRentOutElement != null && decimal.TryParse(numberRegex.Matches(initialCostToRentOutElement.Text)[0].Value, out initialCostToRentOut) ? initialCostToRentOut : (decimal?)null
          
            if (!fundaRecord.RoomCount.HasValue)
            {
                var roomCountRegex = new Regex("([1-9]{1}) kamer(.*)");
                var roomCountElement = this.Driver.FindElementsByCssSelector(".object-primary .object-kenmerken-body .object-kenmerken-list dd").FirstOrDefault(o => roomCountRegex.IsMatch(o.Text));
                if (roomCountElement != null)
                {
                    var parsedRooms = 0;
                    if (int.TryParse(roomCountRegex.Matches(roomCountElement.Text)[0].Groups[1].Value, out parsedRooms))
                    {
                        fundaRecord.RoomCount = parsedRooms;
                    }
                }
            }

            if ((fundaRecord is Sale) && !((Sale)fundaRecord).ServiceCosts.HasValue)
            {
                var serviceCostRegex = new Regex(".* ([0-9]{1,3}) per maand");
                var roomCountElement = this.Driver.FindElementsByCssSelector(".object-primary .object-kenmerken-body .object-kenmerken-list dd").FirstOrDefault(o => serviceCostRegex.IsMatch(o.Text));
                if (roomCountElement != null)
                {
                    var parsedRooms = 0;
                    if (int.TryParse(serviceCostRegex.Matches(roomCountElement.Text)[0].Groups[1].Value, out parsedRooms))
                    {
                        ((Sale)fundaRecord).ServiceCosts = parsedRooms;
                    }
                }
            }


            return fundaRecord;
        }

        public Sale GetSale(IWebElement element)
        {
        var url = element.FindElements(By.CssSelector(".search-result-header a"));
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
            var postCodeRegex = new Regex("([1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2})", RegexOptions.IgnoreCase).Matches(subTitle.Text);
            return new Sale
            {
                Url = url[0].GetAttribute("href"),
                Title = title.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[0],
                Subtitle = subTitle.Text,
                Price = decimal.TryParse(new Regex("([0-9.]*)").Matches(price.Text)[2].Value.Replace(".", ""), out parsedPrice) ? parsedPrice : (decimal?)null,
                LivingArea = int.TryParse(new System.Text.RegularExpressions.Regex("([0-9]*)").Matches(livingArea.Text)[0].Value, out parsedLivingArea) ? parsedLivingArea : (int?)null,
                TotalArea = int.TryParse(new System.Text.RegularExpressions.Regex("([0-9]*)").Matches(totalArea.Text)[0].Value, out parsedTotalArea) ? parsedTotalArea : (int?)null,
                RoomCount = int.TryParse(roomCount.Text.Split(new string[] { "\r\n", "\n", "•", "kamers" }, StringSplitOptions.None)[1].Trim(), out parsedRoomCount) ? parsedRoomCount : (int?)null,
                Address = title.Text.Replace("\r\n", ""),
                PostCode = postCodeRegex.Count != 0 ? postCodeRegex[0].Value : null
            };
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
            var postCodeRegex = new Regex("([1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2})", RegexOptions.IgnoreCase).Matches(subTitle.Text);
            var numberRegex = this.NumberRegex;
            decimal? initialCostToRentOut = null;
            try {
               var initialCostToRentOutElement = element.FindElement(By.CssSelector(".search-result-info-small"));
                initialCostToRentOut = 0m;
            }
            catch { }

            return new Rent
            {
                Url = url[2].GetAttribute("href"),
                Title = title.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[0],
                Subtitle = subTitle.Text,
                Price = decimal.TryParse(numberRegex.Matches(price.Text)[2].Value.Replace(".", ""), out parsedPrice) ? parsedPrice : (decimal?)null,
                LivingArea = int.TryParse(numberRegex.Matches(livingArea.Text)[0].Value, out parsedLivingArea) ? parsedLivingArea : (int?)null,
                TotalArea = int.TryParse(numberRegex.Matches(totalArea.Text)[0].Value, out parsedTotalArea) ? parsedTotalArea : (int?)null,
                RoomCount = int.TryParse(roomCount.Text.Split(new string[] { "\r\n", "\n", "•", "kamers" }, StringSplitOptions.None)[1].Trim(), out parsedRoomCount) ? parsedRoomCount : (int?)null,
                Address = title.Text.Replace("\r\n", ""),
                PostCode = postCodeRegex.Count != 0 ? postCodeRegex[0].Value : null,
                InitialCostToRentOut = initialCostToRentOut 
            };
        }

        public void Dispose()
        {
            this.Driver.Dispose();
        }

        private ChromeDriver Driver { get; set; }
    }
}
