﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Specialized;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace Funda
{
    public class Crawler : IDisposable
    {
        public Crawler()
        {
            var options = new ChromeOptions { };
            options.AddArgument("disable-infobars");
            var username = Environment.GetEnvironmentVariable("USERNAME");
            var userProfile = "C:\\Users\\" + username + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default";
            options.AddArgument(string.Format("user-data-dir={0}", userProfile));

            options.AddArguments("excludeSwitches", "ignore-certificate-errors", "safebrowsing-disable-download-protection", "safebrowsing-disable-auto-update", "disable-client-side-phishing-detection");
            // options.Arguments
            this.Driver = AppDomain.CurrentDomain.BaseDirectory.Contains("ConsoleLauncher") ?
                   new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, options)
               : new ChromeDriver(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Funda\bin\Debug"), options);

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

        public List<Sale> AddNewSales()
        {
            var list = new List<Sale>();
            var adds = this.Driver.FindElementsByCssSelector(".search-result");
            foreach (var advert in adds)
            {
                list.Add(this.GetSale(advert));
            }

            return list;
        }

        public List<Rent> AddNewRents()
        {
            var list = new List<Rent>();
            var adds = this.Driver.FindElementsByCssSelector(".search-result");
            foreach (var advert in adds)
            {
                list.Add(this.GetRent(advert));
            }

            return list;
        }

        public Regex NumberRegex
        {
            get { return new Regex("([0-9.]*)"); }
        }

        public class AruodasSearch : Search
        {
            public override string Sorting { get { return "&FOrder=Actuality"; } }

            public int FDistrict { get; set; }

            public int FRegion { get; set; }

            public bool IsHouse { get { return this.Text.Contains("nam"); } }

            public override bool IsSale
            {
                get
                {
                    return !this.Text.Contains("-nuoma");
                }
            }

            public override string Url
            {
                get
                {
                    //http://www.aruodas.lt/butai/vilniuje/?FDistrict=1&FPriceMax=50000&FPriceMin=0&FRegion=461&FRoomNumMax=4&FRoomNumMin=1&mod=Siulo&act=makeSearch&Page=2
                    string url = "http://www.aruodas.lt/";
                    url += this.Text;
                    url += "?";
                    url += "FDistrict=";
                    url += FDistrict;
                    url += "&obj=" + (this.IsSale && !this.IsHouse ? 1 : this.IsSale && this.IsHouse ? 2 : !this.IsSale && !this.IsHouse ? 4 : 5);
                    url += "&";
                    url += "FPriceMax=";
                    url += PriceMax;
                    url += "&";
                    url += "FPriceMin=";
                    url += PriceMin;
                    url += "&";
                    url += "FRegion=";
                    url += FRegion;
                    url += "&";
                    url += "FRoomNumMax=";
                    url += MaxRooms;
                    url += "&";
                    url += "FRoomNumMin=";
                    url += MinRooms;
                    url += "&";
                    url += "mod=Siulo";
                    url += "&";
                    url += "act=makeSearch";
                    url += this.Sorting;
                    if (this.PaginationNumber.HasValue)
                    {
                        url += string.Format("&Page={0}", this.PaginationNumber.Value);
                    }


                    return url;
                }
            }
        }

        public class FundaSearch : Search
        {
            public override string Sorting { get { return "sorteer-datum-af/"; } }

            public override string Url
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


        public class MestoUaSearch : Search
        {
            public override string Url
            {
                get
                {
                    var url = "https://mesto.ua/uk/sale/" + this.Text + "/?currency=EUR&area_from=1&price_from=" + this.PriceMin + "&rooms=1&rooms=2&rooms=3&rooms=4&rooms=5";
                    if (this.PaginationNumber.HasValue)
                    {
                        url += string.Format("&p={0}", this.PaginationNumber.Value);
                    }
                    return url;
                }
            }
        }

        public class FotoCasaSearch : Search
        {
            public override string Url
            {
                get
                {
                    var url = "https://www.fotocasa.es/es/" + this.Text + "/l";

                    if (this.PaginationNumber.HasValue)
                    {
                        url += string.Format("/{0}", this.PaginationNumber.Value);
                    }
                    url += "?sortType=publicationDate";
                    if (!string.IsNullOrWhiteSpace(this.LatitudeAndLongitude))
                    {
                        url += "&" + this.LatitudeAndLongitude;
                    }
                    if (!string.IsNullOrWhiteSpace(this.CombinedLocationIds))
                    {
                        this.CombinedLocationIds = "&combinedLocationIds=" + this.CombinedLocationIds;
                    }

                    url += "&minPrice=" + this.PriceMin + "&maxPrice=" + this.PriceMax + "&propertySubtypeIds=1;2;5;7;6;8;52;54" + this.CombinedLocationIds + "&gridType=3;";

                    return url;
                }
            }

            public string LatitudeAndLongitude { get; set; }
            public string CombinedLocationIds { get; set; }

        }
        public class PisosSearch : Search
        {
            public override string Url
            {
                get
                {
                    var url = "https://www.pisos.com/";





                    url += this.Text + "/hasta-" + this.PriceMax + "/asc/";
                    if (this.PaginationNumber.HasValue)
                    {
                        url += string.Format("{0}", this.PaginationNumber.Value);
                    }


                    return url;
                }
            }

            public string Extras { get; set; }

        }
        public class Search
        {
            public int? MinRooms { get; set; }
            public int? MaxRooms { get; set; }
            public int? PriceMin { get; set; }
            public int? PriceMax { get; set; }
            public virtual string Sorting { get; }
            public virtual bool IsSale { get; set; }
            public int? PaginationNumber { get; set; }
            public string Text { get; set; }
            public virtual string Url { get; }
        }

        public IRecord GetRecordDataFromItsPage(IRecord fundaRecord)
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

                try
                {
                    var IsFurnishedSekcijos = this.Driver.FindElementsByCssSelector(".object-kenmerken-body .object-kenmerken-list");
                    var IsFurnishedSekcija = IsFurnishedSekcijos.FirstOrDefault(o => o.Text.Contains("Specifiek"));
                    if (IsFurnishedSekcija != null)
                    {
                        var isFurishedElementai = IsFurnishedSekcija.FindElements(By.CssSelector("dt"));
                        var isFurnishedElementas = isFurishedElementai.FirstOrDefault(o => o.Text.Contains("Specifiek"));
                        var isFurnishedText = IsFurnishedSekcija.FindElements(By.CssSelector("dd"))[isFurishedElementai.IndexOf(isFurnishedElementas)];
                        ((Rent)fundaRecord).IsFurnished = isFurnishedText.Text == "Gemeubileerd";
                    }

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
                var serviceCostElement = this.Driver.FindElementsByCssSelector(".object-primary .object-kenmerken-body .object-kenmerken-list dd").FirstOrDefault(o => serviceCostRegex.IsMatch(o.Text));
                if (serviceCostElement != null)
                {
                    var serviceCost = 0;
                    if (int.TryParse(serviceCostRegex.Matches(serviceCostElement.Text)[0].Groups[1].Value, out serviceCost))
                    {
                        ((Sale)fundaRecord).ServiceCosts = serviceCost;
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
            try
            {
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

        public List<Sale> AddNewLtSales(AruodasSearch search)
        {
            var list = new List<Sale>();
            var adds = this.Driver.FindElementsByClassName("list-row").Where(o => o.Text != "");
            foreach (var advert in adds)
            {
                list.Add(this.GetLtSale(advert, search));
            }

            return list;
        }

        private Sale GetLtSale(IWebElement element, AruodasSearch search)
        {
            var url = element.FindElement(By.CssSelector("a")).GetAttribute("href");
            var title = element.FindElement(By.CssSelector(".list-adress ")).Text;
            //    var subTitle = element.FindElement(By.CssSelector(".search-result-subtitle"));
            var price = "";
            var livingArea = "";
            var roomCount = "";
            var totalArea = "";
            if (search.IsHouse)
            {
                price = element.FindElement(By.CssSelector(".list-item-price")).Text.Replace(" ", "").Replace("€", "");
                livingArea = element.FindElements(By.CssSelector(".list-row td"))[2].Text;
                totalArea = element.FindElements(By.CssSelector(".list-row td"))[3].Text;
            }
            else
            {
                price = element.FindElement(By.CssSelector(".list-item-price")).Text.Replace(" ", "").Replace("€", "");
                livingArea = element.FindElements(By.CssSelector(".list-row td"))[3].Text;
                roomCount = element.FindElements(By.CssSelector(".list-row td"))[2].Text;
            }

            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            var parsedTotalArea = 0M;
            var parsedRoomCount = 0;
            //   var postCodeRegex = new Regex("([1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2})", RegexOptions.IgnoreCase).Matches(subTitle.Text);
            return new Sale
            {
                Url = url,
                Title = title,
                // Subtitle = subTitle.Text,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                TotalArea = decimal.TryParse(totalArea, out parsedTotalArea) ? (int?)(parsedTotalArea * 1000) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                //  Address = title.Text.Replace("\r\n", ""),
                //   PostCode = postCodeRegex.Count != 0 ? postCodeRegex[0].Value : null
                //"1233"
            };
        }

        public List<Rent> AddNewLtRents()
        {
            var list = new List<Rent>();
            var adds = this.Driver.FindElementsByClassName("list-row").Where(o => o.Text != "");
            foreach (var advert in adds)
            {
                list.Add(this.GetLtRent(advert));
            }

            return list;
        }

        private Rent GetLtRent(IWebElement element)
        {
            var url = element.FindElement(By.CssSelector("a")).GetAttribute("href");
            var title = element.FindElement(By.CssSelector(".list-adress ")).Text;
            //    var subTitle = element.FindElement(By.CssSelector(".search-result-subtitle"));
            var price = element.FindElement(By.CssSelector(".list-item-price")).Text.Replace(" ", "").Replace("€", "");
            var livingArea = element.FindElements(By.CssSelector(".list-row td"))[3].Text;
            var roomCount = element.FindElements(By.CssSelector(".list-row td"))[2].Text;
            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            //    var parsedTotalArea = 0;
            var parsedRoomCount = 0;
            //   var postCodeRegex = new Regex("([1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2})", RegexOptions.IgnoreCase).Matches(subTitle.Text);
            return new Rent
            {
                Url = url,
                Title = title,
                // Subtitle = subTitle.Text,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                //  Address = title.Text.Replace("\r\n", ""),
                //   PostCode = postCodeRegex.Count != 0 ? postCodeRegex[0].Value : null
                //"1233"
            };
        }

        public string[] GetAruodasAddress()
        {

            var mapElement = this.Driver.FindElementsByCssSelector("[href='#map']");
            if (mapElement.Any())
            {
                using (var mapDriver = new ChromeDriver(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Driver\")))
                {
                    try
                    {
                        var mapUrl = mapElement[0].GetAttribute("data-href");
                        mapDriver.Url = string.Format("http://www.aruodas.lt{0}", mapUrl);
                        mapDriver.Navigate();
                        Thread.Sleep(1000);
                        mapDriver.FindElementByCssSelector("[title^='Spustel']").Click();
                        Thread.Sleep(2500);
                        mapDriver.SwitchTo().Window(mapDriver.WindowHandles.Last());
                        Actions action = new Actions(mapDriver);
                        action.MoveToElement(mapDriver.FindElementByCssSelector("body"), 464, 467).ContextClick().Build().Perform();
                        Thread.Sleep(2000);
                        action.SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.Return).Build().Perform();
                        Thread.Sleep(1000);
                        var address = mapDriver.FindElementsByCssSelector(".widget-reveal-card-address-line");

                        return address.Select(o => o.Text).ToArray();
                    }
                    finally
                    {
                        mapDriver.Close();
                    }
                }
            }

            return null;
        }

        public IRecord GetRecordDataFromFotoCasa(IRecord record)
        {
            record = MarkSoldFotoCasa(record);
            if (record.SpatialAnalysis.Count == 0)
            {
                var lat = string.Empty;
                var lon = string.Empty;
                var scriptWithCoord = this.Driver.FindElements(By.CssSelector("script")).Where(o => o.GetAttribute("innerHTML").Contains("window.__INITIAL_PROPS__"));
                if (scriptWithCoord.Any())
                {
                    var coords = scriptWithCoord.First().GetAttribute("innerHTML").Split(new[] { "coordinates", "accuracy" }, StringSplitOptions.None)[2].Split(':');
                    lat = coords[2].Split(',')[0];
                    lon = coords[3].Split(',')[0];
                }
                else
                {
                    scriptWithCoord = this.Driver.FindElements(By.CssSelector("script")).Where(o => o.GetAttribute("innerHTML").Contains("\"Lat\""));
                    if (scriptWithCoord.Any())
                    {
                        var coords = scriptWithCoord.First().GetAttribute("innerHTML").Split(new[] { "Lat\": ", ",\"oasGeoPostalCode" }, StringSplitOptions.None)[1].Split(new[] { ",\"Lng\": " }, StringSplitOptions.None);
                        lat = coords[0].Trim('"');
                        lon = coords[1].Trim('"');
                    }
                }

                if (!string.IsNullOrWhiteSpace(lat) && !string.IsNullOrWhiteSpace(lat))
                {
                    record.SpatialAnalysis.Add(new SpatialAnalysis() { Point = System.Data.Entity.Spatial.DbGeography.PointFromText($"POINT({lat} {lon})", 4326) });
                }
            }
            record.DateLastProcessed = DateTime.Now;
            return record;
        }

        public IRecord GetRecordDataFromItsPageLt(IRecord record)
        {
            if (!record.DateAdded.HasValue)
            {
                DateTime? dateAdded = null;
                //   DateTime date = DateTime.Parse("yyyy-MM-dd");
                var dateAddedElement = this.Driver.FindElementsByCssSelector(".obj-stats.obj-stats-border dd");

                if (dateAddedElement.Count > 1)
                {
                    System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("lt-LT");
                    dateAdded = DateTime.Parse(dateAddedElement[1].Text, cultureinfo);
                }
                else
                {
                    dateAdded = DateTime.Now;
                }

                record.DateAdded = dateAdded;
            }

            if (!record.DateRemoved.HasValue)
            {
                DateTime? dateRemoved;
                if (record.Url != this.Driver.Url)
                {
                    dateRemoved = DateTime.Now;
                }
                else
                {
                    try
                    {
                        var dateRemovedElement = this.Driver.FindElementByCssSelector(".error-div.error2");
                        dateRemoved = DateTime.Now;
                    }
                    catch
                    {
                        dateRemoved = null;
                    }
                }

                record.DateRemoved = dateRemoved;
            }
            if (string.IsNullOrEmpty(record.HeatingType))
            {

                var sildymasElementai = this.Driver.FindElementsByCssSelector(".obj-details dt");
                var sildymasElementas = sildymasElementai.FirstOrDefault(o => o.Text.Contains("ildymas:"));
                if (sildymasElementas != null)
                {
                    var heatingType = this.Driver.FindElementsByCssSelector(".obj-details dd")[sildymasElementai.IndexOf(sildymasElementas)];
                    if (heatingType.Text.Length < 50)
                        record.HeatingType = heatingType.Text;
                }
            }

            try
            {
                var priceElement = this.Driver.FindElementByCssSelector(".obj-price");
                var price = int.Parse(priceElement.Text.Split('€')[0]);
                if (record.Price != price)
                {
                    record.Price = price;
                }

            }
            catch { }

            if (!record.IsBendrabutis.HasValue)
            {
                var mainText = this.Driver.FindElementsByClassName("obj-comment");
                if (mainText.Any())
                {
                    record.IsBendrabutis = mainText[0].Text.ToLower().Contains("bendrabu");
                }
            }

            record.DateLastProcessed = DateTime.Now;
            return record;
        }

        public List<Sale> AddNewMestoUeSales(MestoUaSearch search)
        {
            var list = new List<Sale>();
            var adds = this.Driver.FindElementsByClassName("title");
            foreach (var advert in adds)
            {
                list.Add(this.GetMestoUaSale(advert, search));
            }

            return list;
        }

        private Sale GetMestoUaSale(IWebElement element, MestoUaSearch search)
        {
            var url = element.FindElement(By.CssSelector("a")).GetAttribute("href");
            var title = element.FindElement(By.CssSelector(".list-adress ")).Text;
            //    var subTitle = element.FindElement(By.CssSelector(".search-result-subtitle"));
            var price = "";
            var livingArea = "";
            var roomCount = "";
            var totalArea = "";
            //if (search.IsHouse)
            //{
            //    price = element.FindElement(By.CssSelector(".list-item-price")).Text.Replace(" ", "").Replace("€", "");
            //    livingArea = element.FindElements(By.CssSelector(".list-row td"))[2].Text;
            //    totalArea = element.FindElements(By.CssSelector(".list-row td"))[3].Text;
            //}
            //else
            //{
            price = element.FindElement(By.CssSelector(".list-item-price")).Text.Replace(" ", "").Replace("€", "");
            livingArea = element.FindElements(By.CssSelector(".list-row td"))[3].Text;
            roomCount = element.FindElements(By.CssSelector(".list-row td"))[2].Text;
            //  }

            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            var parsedTotalArea = 0M;
            var parsedRoomCount = 0;
            //   var postCodeRegex = new Regex("([1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2})", RegexOptions.IgnoreCase).Matches(subTitle.Text);
            return new Sale
            {
                Url = url,
                Title = title,
                // Subtitle = subTitle.Text,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                TotalArea = decimal.TryParse(totalArea, out parsedTotalArea) ? (int?)(parsedTotalArea * 1000) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                //  Address = title.Text.Replace("\r\n", ""),
                //   PostCode = postCodeRegex.Count != 0 ? postCodeRegex[0].Value : null
                //"1233"
            };
        }


        public List<Rent> AddFotoCasaRents()
        {
            var list = new List<Rent>();
            (this.Driver as IJavaScriptExecutor).ExecuteScript("const delay = ms => new Promise(resolve => setTimeout(resolve, ms)); " +
                        "(async () => {" +
                        "    while (document.documentElement.scrollTop <= document.body.scrollHeight - 500)" +
                        "    {" +
                        "        window.scrollTo(0, document.documentElement.scrollTop + 500);" +
                        "        await delay(300);" +
                        "    }" +
                        "})(); ");
            Thread.Sleep(5500);

            var adds = this.Driver.FindElements(By.CssSelector(".re-Searchresult-itemRow .re-Card")).Where(o => o.Text != "");
            foreach (var advert in adds)
            {
                try
                {
                    list.Add(this.GetFotoCasaRents(advert));
                }
                catch (Exception e)
                {

                }
            }

            return list;
        }

        private Rent GetFotoCasaRents(IWebElement element)
        {

            var url = element.FindElement(By.CssSelector(".re-Card-link")).GetAttribute("href").Split('?')[0];
            var title = string.Join(" ", url.Split('/')[6].Split('-'));
            // var subTitle = string.Join(" ", url.Split('/')[7].Split('-'));

            var price = element.FindElement(By.CssSelector(".re-Card-price")).Text.Split(new[] { "<span" }, StringSplitOptions.None)[0].Split(' ')[0];
            var roomsAndArea = element.FindElements(By.CssSelector(".re-Card-feature"));
            var roomCountRegex = new Regex("([1-9]{1}) hab(s)\\.");
            var livingAreaRegex = new Regex("([1-9][0-9]{1,2}) m²");
            var roomCountElement = roomsAndArea.Where(o => roomCountRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingAreaElement = roomsAndArea.Where(o => livingAreaRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingArea = livingAreaElement != null ? livingAreaElement.Text.Split(' ')[0] : null;

            var roomCount = roomCountElement != null ? roomCountElement.Text.Split(' ')[0] : null;
            var dateAddedText = element.FindElement(By.CssSelector(".re-Card-timeago")).Text;

            int parsedDateAdded = 0;
            int.TryParse(dateAddedText.Split(' ')[1], out parsedDateAdded);
            var dateAdded = dateAddedText.Split(' ')[2].StartsWith("d", StringComparison.CurrentCultureIgnoreCase)
                            ? DateTime.Now.Subtract(new TimeSpan(parsedDateAdded, 0, 0, 0)) : DateTime.Now;

            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            var parsedRoomCount = 0;

            return new Rent
            {
                Url = url,
                Title = title,
                //   Subtitle = subTitle,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice <= 2 ? parsedPrice * 1000 : parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                DateAdded = dateAdded
            };

        }
        public List<Sale> AddFotoCasaSales()
        {
            var list = new List<Sale>();
            (this.Driver as IJavaScriptExecutor).ExecuteScript("const delay = ms => new Promise(resolve => setTimeout(resolve, ms)); " +
                        "(async () => {" +
                        "    while (document.documentElement.scrollTop <= document.body.scrollHeight - 500)" +
                        "    {" +
                        "        window.scrollTo(0, document.documentElement.scrollTop + 500);" +
                        "        await delay(300);" +
                        "    }" +
                        "})(); ");
            Thread.Sleep(5500);

            var adds = this.Driver.FindElements(By.CssSelector(".re-Searchresult-itemRow .re-Card")).Where(o => o.Text != "");
            foreach (var advert in adds)
            {
                try
                {
                    list.Add(this.GetFotoCasaSales(advert));
                }
                catch (Exception e)
                {

                }
            }

            return list;
        }

        private Sale GetFotoCasaSales(IWebElement element)
        {

            var url = element.FindElement(By.CssSelector(".re-Card-link")).GetAttribute("href").Split('?')[0];
            var title = string.Join(" ", url.Split('/')[6].Split('-'));
            // var subTitle = string.Join(" ", url.Split('/')[7].Split('-'));


            var price = element.FindElement(By.CssSelector(".re-Card-price")).Text.Split(new[] { "<span" }, StringSplitOptions.None)[0].Split(' ')[0].Replace(".", "");
            var roomsAndArea = element.FindElements(By.CssSelector(".re-Card-feature"));
            var roomCountRegex = new Regex("([1-9]{1}) hab(s)\\.");
            var livingAreaRegex = new Regex("([1-9][0-9]{1,2}) m²");
            var roomCountElement = roomsAndArea.Where(o => roomCountRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingAreaElement = roomsAndArea.Where(o => livingAreaRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingArea = livingAreaElement != null ? livingAreaElement.Text.Split(' ')[0] : null;

            var roomCount = roomCountElement != null ? roomCountElement.Text.Split(' ')[0] : null;
            var dateAddedText = element.FindElement(By.CssSelector(".re-Card-timeago")).Text;

            int parsedDateAdded = 0;
            int.TryParse(dateAddedText.Split(' ')[1], out parsedDateAdded);
            var dateAdded = dateAddedText.Split(' ')[2].StartsWith("d", StringComparison.CurrentCultureIgnoreCase)
                            ? DateTime.Now.Subtract(new TimeSpan(parsedDateAdded, 0, 0, 0)) : DateTime.Now;

            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            var parsedRoomCount = 0;

            return new Sale
            {
                Url = url,
                Title = title,
                //   Subtitle = subTitle,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice <= 2 ? parsedPrice * 1000 : parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                DateAdded = dateAdded
            };

        }
        public string ChangePisosTitle(string caption)
        {
            if (caption == "El Puerto de Santa María, Cádiz") { return "el puerto de santa maria"; }
            else if (caption == "Huelva") { return "huelva capital"; }
            else if (caption == "Algeciras, Cádiz") { return "algeciras"; }
            else if (caption == "Estepona, Málaga") { return "estepona"; }
            else if (caption == "Marbella, Málaga") { return "marbella"; }
            else if (caption == "Málaga") { return "malaga capital"; }
            else if (caption == "Roquetas de Mar, Almería") { return "roquetas de mar"; }
            else if (caption == "Almería") { return "almeria capital"; }
            else if (caption == "Torrevieja, Alicante") { return "torrevieja"; }
            else if (caption == "Alicante") { return "alicante alacant"; }
            else if (caption == "Benidorm, Alicante") { return "benidorm"; }
            else if (caption == "Dénia, Alicante") { return "denia"; }
            else if (caption == "Gandia, València") { return "gandia"; }
            else if (caption == "Cullera, València") { return "cullera"; }
            else if (caption == "València") { return "valencia capital"; }
            else if (caption == "Daimús, València") { return "daimus"; }
            else if (caption == "Motril, Granada") { return "motril"; }
            else if (caption == "Águilas, Murcia") { return "aguilas"; }
            return caption;
        }
        public List<Rent> AddPisosRents()
        {
            var list = new List<Rent>();

            for (int i = 0; i < 20; i++)
            {


                var adds = this.Driver.FindElements(By.CssSelector(".row-to-hide")).Where(o => o.Text != "");
                var title = this.Driver.FindElements(By.CssSelector("b"))[0].Text;
                foreach (var advert in adds)
                {

                    try
                    {
                        var ad = this.GetPisosRents(advert);

                        ad.Title = ChangePisosTitle(title);
                        if (list.All(a => a.Url != ad.Url))
                        {
                            list.Add(ad);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                    (this.Driver as IJavaScriptExecutor).ExecuteScript(" window.scrollTo(0, document.documentElement.scrollTop + 500);");
                Thread.Sleep(500);
            }
            return list;
        }

        private Rent GetPisosRents(IWebElement element)
        {

            var url = element.FindElements(By.CssSelector("meta[itemprop = url]")).First().GetAttribute("content");
            //var title = string.Join(" ", url.Split('/')[6].Split('-'));


            var price = element.FindElement(By.CssSelector(".price")).Text.Split(new[] { "<span" }, StringSplitOptions.None)[0].Split(' ')[0];
            var roomsAndArea = element.FindElements(By.CssSelector(".item"));
            var roomCountChildElement = element.FindElement(By.CssSelector(".icoBed"));

            var livingAreaRegex = new Regex("([1-9][0-9]{1,2}) m²");
            var roomCountElement = roomCountChildElement.FindElement(By.XPath("..")); // roomsAndArea.Where(o => roomCountRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingAreaElement = roomsAndArea.Where(o => livingAreaRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingArea = livingAreaElement != null ? livingAreaElement.Text.Split(' ')[0] : null;

            var roomCount = roomCountElement != null ? roomCountElement.Text.Split(' ')[0] : null;




            var dateAdded = DateTime.Now;


            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            var parsedRoomCount = 0;

            return new Rent
            {
                Url = url,
                //Title = title,
                //  Subtitle = subTitle,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice <= 2 ? parsedPrice * 1000 : parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                DateAdded = dateAdded
            };

        }

        public List<Sale> AddPisosSales()
        {
            var list = new List<Sale>();
            //(this.Driver as IJavaScriptExecutor).ExecuteScript("const delay = ms => new Promise(resolve => setTimeout(resolve, ms)); " +
            //            "(async () => {" +
            //            "    while (document.documentElement.scrollTop <= document.body.scrollHeight - 500)" +
            //            "    {" +
            //            "        window.scrollTo(0, document.documentElement.scrollTop + 500);" +
            //            "        await delay(300);" +
            //            "    }" +
            //            "})(); ");
            //Thread.Sleep(5500);

            for (int i = 0; i < 20; i++)
            {


                var adds = this.Driver.FindElements(By.CssSelector(".row-to-hide")).Where(o => o.Text != "");
                var title = this.Driver.FindElements(By.CssSelector("b"))[0].Text;
                foreach (var advert in adds)
                {

                    try
                    {
                        var ad = this.GetPisosSales(advert);

                        ad.Title = ChangePisosTitle(title);
                        if (list.All(a => a.Url != ad.Url))
                        {
                            list.Add(ad);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                             (this.Driver as IJavaScriptExecutor).ExecuteScript(" window.scrollTo(0, document.documentElement.scrollTop + 500);");
                Thread.Sleep(500);
            }
            return list;
        }

        private Sale GetPisosSales(IWebElement element)
        {

            var url = element.FindElements(By.CssSelector("meta[itemprop = url]")).First().GetAttribute("content");
            //var title = string.Join(" ", url.Split('/')[6].Split('-'));



            var price1 = element.FindElement(By.CssSelector(".price")).Text.Split(new[] { "<span" }, StringSplitOptions.None)[0].Split(' ')[0].Split('.')[0];
            var price2 = element.FindElement(By.CssSelector(".price")).Text.Split(new[] { "<span" }, StringSplitOptions.None)[0].Split(' ')[0].Split('.')[1];
            var price = price1 + "" + price2;

            var roomsAndArea = element.FindElements(By.CssSelector(".item"));
            var roomCountChildElement = element.FindElement(By.CssSelector(".icoBed"));

            var livingAreaRegex = new Regex("([1-9][0-9]{1,2}) m²");
            var roomCountElement = roomCountChildElement.FindElement(By.XPath("..")); // roomsAndArea.Where(o => roomCountRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingAreaElement = roomsAndArea.Where(o => livingAreaRegex.IsMatch(o.Text)).FirstOrDefault();
            var livingArea = livingAreaElement != null ? livingAreaElement.Text.Split(' ')[0] : null;

            var roomCount = roomCountElement != null ? roomCountElement.Text.Split(' ')[0] : null;




            var dateAdded = DateTime.Now;


            var parsedPrice = 0M;
            var parsedLivingArea = 0M;
            var parsedRoomCount = 0;

            return new Sale
            {
                Url = url,
                //Title = title,
                //   Subtitle = subTitle,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice <= 2 ? parsedPrice * 1000 : parsedPrice : (decimal?)null,
                LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                DateAdded = dateAdded
            };

        }

        public IRecord MarkSoldFotoCasa(IRecord record)
        {
            if (this.Driver.FindElementsByCssSelector(".re-Searchpage-propertyNotFound").Any() || this.Driver.Url!= record.Url)
                record.DateRemoved = DateTime.Now;
            return record;
        }

        public class DaftieSearch : Search
        {
            public override string Url
            {
                get
                {
                    var url = "https://www.daft.ie/";
                    url += this.Text;
                    url += IsSale? "/property-for-sale/?mxp=250000&advanced=1&pt_id%5B0%5D=2&pt_id%5B1%5D=3&pt_id%5B2%5D=6&sort_type=d&cc_id=ct2&s%5Bmxp%5D=250000&s%5Badvanced%5D=1&s%5Bpt_id%5D%5B0%5D=2&s%5Bpt_id%5D%5B1%5D=3&s%5Bpt_id%5D%5B2%5D=6&s%5Bsort_type%5D=d&s%5Bsort_by%5D=date&searchSource=sale":
                        "/residential-property-for-rent/?ad_type=rental&advanced=1&s%5Bmxp%5D=2000&s%5Badvanced%5D=1&s%5Bpt_id%5D%5B0%5D=1&s%5Bpt_id%5D%5B1%5D=3&s%5Bpt_id%5D%5B2%5D=4&rental_tab_name=advanced-sf&searchSource=rental";
                    if (this.PaginationNumber.HasValue)
                    {
                        url += "&offset=" + string.Format("{0}", this.PaginationNumber.Value * 20);
                    }

                    return url;
                }
            }

        }

        public List<IRecord> AddDaft(bool isSale)
        {
            var list = new List<IRecord>();
            (this.Driver as IJavaScriptExecutor).ExecuteScript("const delay = ms => new Promise(resolve => setTimeout(resolve, ms)); " +
                        "(async () => {" +
                        "    while (document.documentElement.scrollTop <= document.body.scrollHeight - 500)" +
                        "    {" +
                        "        window.scrollTo(0, document.documentElement.scrollTop + 500);" +
                        "        await delay(300);" +
                        "    }" +
                        "})(); ");
            Thread.Sleep(3000);
            var selectorClass = isSale ? "AdCard__adCardContainer" : "box";
            var adds =  this.Driver.FindElements(By.CssSelector("."+selectorClass)).Where(o => o.Text != "") ;
            foreach (var advert in adds)
            {
                try
                {
                    if (isSale)
                    {
                        list.Add(this.GetDaftSales(advert));
                    }
                    else {
                        list.Add(this.GetDaftRents(advert));
                    }
                }
                catch (Exception e)
                {

                }
            }


            return list;
        }
        private Sale GetDaftSales(IWebElement element)
        {

            var url =  element.FindElement(By.CssSelector(".PropertyInformationCommonStyles__propertyPrice--link")).GetAttribute("href");
            //var title = string.Join(" ", url.Split('/')[6].Split('-'));
            var address = element.FindElement(By.CssSelector(".PropertyInformationCommonStyles__addressCopy--link")).Text;


            var price = element.FindElement(By.CssSelector(".PropertyInformationCommonStyles__costAmountCopy")).Text.Replace("€", string.Empty).Replace(",", string.Empty);
            //var roomsAndArea = element.FindElements(By.CssSelector(".re-Card-feature"));
            //var roomCountRegex = new Regex("([1-9]{1}) hab(s)\\.");
            //var livingAreaRegex = new Regex("([1-9][0-9]{1,2}) m²");
            var roomCountElement = element.FindElement(By.CssSelector(".QuickPropertyDetails__iconCopy"));
            //var livingAreaElement = roomsAndArea.Where(o => livingAreaRegex.IsMatch(o.Text)).FirstOrDefault();
            //var livingArea = livingAreaElement != null ? livingAreaElement.Text.Split(' ')[0] : null;

            var roomCount = roomCountElement != null ? roomCountElement.Text : null;
            //var dateAddedText = element.FindElement(By.CssSelector(".re-Card-timeago")).Text;

            //int parsedDateAdded = 0;
            //int.TryParse(dateAddedText.Split(' ')[1], out parsedDateAdded);
            //var dateAdded = dateAddedText.Split(' ')[2].StartsWith("d", StringComparison.CurrentCultureIgnoreCase)
            //                ? DateTime.Now.Subtract(new TimeSpan(parsedDateAdded, 0, 0, 0)) : DateTime.Now;

            var parsedPrice = 0M;
            //  var parsedLivingArea = 0M;
            var parsedRoomCount = 0;

            return new Sale
            {
                Url = url,
                //Title = title,
                Address = address,
                Price = decimal.TryParse(price, out parsedPrice) ? parsedPrice : (decimal?)null,
                //   LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                DateAdded = DateTime.Now
            };

        }
        private Rent GetDaftRents(IWebElement element)
        {

            var url =  element.FindElement(By.CssSelector(".search_result_title_box h2 a")).GetAttribute("href");
            var address = element.FindElement(By.CssSelector(".search_result_title_box h2 a")).Text.Split(new[] { " - " },StringSplitOptions.None)[0];

            var priceElement = element.FindElement(By.CssSelector(".price"));
            var priceText = priceElement.Text.Split(' ')[0].Replace("€", string.Empty).Replace(",", string.Empty);
            var priceIsInMonths = priceElement.Text.Contains("month");
            var rooms = element.FindElements(By.CssSelector(".info li"));
            var roomCountRegex = new Regex("([0-9] Bed(s)*)");
            var roomCountElement = rooms.Where(o => roomCountRegex.IsMatch(o.Text)).FirstOrDefault();
            var roomCount = roomCountElement != null ? roomCountElement.Text.Trim().Split(' ')[0] : null;
            if (roomCount == null && rooms.Any(o => o.Text.Contains("Studio")))
            {
                roomCount = "0";
            }

            var parsedPrice = 0M;

            var parsedRoomCount = 0;

            return new Rent
            {
                Url = url,
                //Title = title,
                Address = address,
                Price = decimal.TryParse(priceText, out parsedPrice) ? priceIsInMonths ? parsedPrice : (parsedPrice * 4.3452m) : (decimal?)null,
                //   LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null,
                RoomCount = int.TryParse(roomCount, out parsedRoomCount) ? parsedRoomCount : (int?)null,
                DateAdded = DateTime.Now
            };

        }
         public IRecord GetRecordDataFromDaftSale(IRecord record)
        {

            var livingAreaElement = this.Driver.FindElements(By.CssSelector(".PropertyOverview__propertyOverviewDetails")).FirstOrDefault();
            var livingAreaText = livingAreaElement.Text.Split(new[] { "Overall Floor Area: " }, StringSplitOptions.None);
            var livingArea = livingAreaElement != null ? livingAreaText.Length > 1 ? livingAreaText[1].Split(new[] { " m2" }, StringSplitOptions.None)[0] : null : null;
            var parsedLivingArea = 0M;
            var addressElement = this.Driver.FindElements(By.CssSelector(".PropertyMainInformation__eircode")).FirstOrDefault();
            record.PostCode = addressElement !=null ? addressElement.Text.Split(new[] { "Eircode: " }, StringSplitOptions.None)[1] : null;

            record.LivingArea = decimal.TryParse(livingArea, out parsedLivingArea) ? (int?)Math.Round(parsedLivingArea, 0) : (int?)null;
            record.DateLastProcessed = DateTime.Now;
            return record;
        }

        public IRecord GetRecordDataFromDaftRent(IRecord record)
        {
            if (this.Driver.FindElementsByCssSelector(".warning").Any(o => o.Text.Contains("This Property Has been either let or withdrawn ")))
            {
                record.DateRemoved = DateTime.Now;
            }

            var priceElement = this.Driver.FindElement(By.CssSelector("#smi-price-string"));
            var priceText = priceElement.Text.Split(' ')[0].Replace("€", string.Empty).Replace(",", string.Empty);
            var priceIsInMonths = priceElement.Text.Contains("month");
            if (!priceIsInMonths) {
                var parsedPrice = 0M;
                record.Price = record.Price = decimal.TryParse(priceText, out parsedPrice) ? priceIsInMonths ? parsedPrice : (parsedPrice * 4.3452m) : (decimal?)null;
            }

            var areaRegex = new Regex("([0-9]{2,4}|[0-9,.]{5}) ?(square|sq)([. ]{1,2})?(meters|met|m|feet|ft|f)");
            var areaElement = this.Driver.FindElementsByCssSelector(".smi-tab-content").FirstOrDefault(o => areaRegex.IsMatch(o.Text));
            if (areaElement != null)
            {
                decimal parsedArea = 0M;
                if (decimal.TryParse(areaRegex.Matches(areaElement.Text)[0].Groups[1].Value.Replace(",",string.Empty), out parsedArea))
                {
                    record.LivingArea =  new[] { "feet", "ft", "f" }.Any(o=> areaRegex.Matches(areaElement.Text)[0].Value.Contains(o)) ? (int?)(parsedArea/(10.764m)) : (int?)parsedArea;
                }
            }

            record.DateLastProcessed = DateTime.Now;
            return record;
        }
    }
}
