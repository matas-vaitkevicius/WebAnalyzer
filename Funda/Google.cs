using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Funda
{
    public class Google
    {
        public  DbGeography CreatePoint(decimal lat, decimal lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }
        public void CallGooglePlacesAPIAndSetCallback(string websiteName)
        {
           // System.Data.Entity.SqlServer.SqlProviderServices.SqlServerTypesAssemblyName = typeof(SqlGeography).Assembly.FullName;
            // if (!File.Exists("results.csv")) { File.CreateText("results.csv"); }
            // var keywords = "(" + string.Join(") OR (", ConfigurationManager.AppSettings.Get("keywords").Split(new[] { ',' })) + ")";
            var googlePlacesApiKey = "AIzaSyA5m6qBfiijwxzTcRsuVmtL-1dMKm-cUWM";
            //  var radius = ConfigurationManager.AppSettings.Get("radius");
            //   string filename = ConfigurationManager.AppSettings.Get("coordinateSource");
            using (var db = new Funda.WebAnalyzerEntities())
            {
                IList<IRecord> addressesToBeSearched = db.Rent.Where<IRecord>(o => o.Url.Contains(websiteName) && !o.SpatialAnalysis.Any(x => x.Rent!=null)).Union(db.Rent.Where<IRecord>(oo => oo.Url.Contains(websiteName) && !oo.SpatialAnalysis.Any(xx => xx.Sale != null))).Distinct().ToList();

                foreach (var locationTobeSearched in addressesToBeSearched)
                {
                    try
                    {
                        dynamic res = null;
                        using (var client = new HttpClient())
                        {
                            while (res == null || HasProperty(res, "next_page_token"))
                            {
                                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={locationTobeSearched.Address}&key={googlePlacesApiKey}&bounds=51.222,-11.0133788|55.636,-5.6582363";
                                if (res != null && HasProperty(res, "next_page_token"))
                                    url += "&pagetoken=" + res["next_page_token"];
                                var response = client.GetStringAsync(url).Result;
                                JavaScriptSerializer json = new JavaScriptSerializer();
                                res = json.Deserialize<dynamic>(response);


                                if (res["status"] == "OK")
                                {
                                    foreach (var match in res["results"])
                                    {
                                        //  if (!File.ReadAllText("results.csv").Contains(match["place_id"]))
                                        //  {
                                        //      var placeResponse = client.GetStringAsync(string.Format("https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}", match["place_id"], googlePlacesApiKey)).Result;
                                        Tuple<decimal?, decimal?, string> coordinatesAndPostCode = ReadResponse(match);
                                        if (coordinatesAndPostCode != null && coordinatesAndPostCode.Item1.HasValue && coordinatesAndPostCode.Item2.HasValue)
                                        {
                                            locationTobeSearched.SpatialAnalysis.Add(new SpatialAnalysis() { Point = CreatePoint(coordinatesAndPostCode.Item1.Value, coordinatesAndPostCode.Item2.Value) });
                                            locationTobeSearched.PostCode = coordinatesAndPostCode.Item3;
                                        }
                                        //        }
                                    }
                                }
                                else if (res["status"] == "OVER_QUERY_LIMIT")
                                {
                                    return;
                                }


                            }


                        }
                    }
                    catch (Exception e)
                    {

                    }

                    db.SaveChanges();
                }
            }
        }

     

        public  Tuple<decimal?,decimal?,string> ReadResponse(dynamic res)
        {

            try
            {
                var latitude = (decimal)res["geometry"]["location"]["lat"];
                var longitude = (decimal)res["geometry"]["location"]["lng"];
                var postcode = (string)GetPostalCode(res);

                return new Tuple<decimal?, decimal?, string>(latitude, longitude, postcode);
            }
            catch (Exception e){
                return null;
            }
        }

        public string GetPostalCode(dynamic res)
        {

            foreach (var component in res["address_components"])
            {
                foreach (var type in component["types"])
                {
                        if (type == "postal_code")
                        return component["long_name"];
                }
            }

            return null;
        }


        public  bool HasProperty(dynamic obj, string name)
        {
            try
            {
                var value = obj[name];
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
