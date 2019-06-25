using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Funda
{
    public class Google
    {
        public static void CallGooglePlacesAPIAndSetCallback()
        {
            // if (!File.Exists("results.csv")) { File.CreateText("results.csv"); }
            // var keywords = "(" + string.Join(") OR (", ConfigurationManager.AppSettings.Get("keywords").Split(new[] { ',' })) + ")";
            var googlePlacesApiKey = ConfigurationManager.AppSettings.Get("googlePlacesApiKey");
            //  var radius = ConfigurationManager.AppSettings.Get("radius");
            //   string filename = ConfigurationManager.AppSettings.Get("coordinateSource");
            using (var db = new Funda.WebAnalyzerEntities())
            {
                var addresseToBeSearched = db.Rent.Where<IRecord>(o => o.PostCode == null).Union(db.Rent.Where<IRecord>(oo => oo.PostCode == null)).Distinct();

                foreach (var locationTobeSearched in addresseToBeSearched)
                {
                    try
                    {
                        dynamic res = null;
                        using (var client = new HttpClient())
                        {
                            while (res == null || HasProperty(res, "next_page_token"))
                            {
                                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={locationTobeSearched}&key={googlePlacesApiKey}&bounds=51.222,-11.0133788|55.636,-5.6582363";
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
                                        var getCoordinatesAndPostCode = ReadResponse(match);

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
                }
            }
        }

     

        public static Tuple<decimal,decimal,string> ReadResponse(string response)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var res = json.Deserialize<dynamic>(response);
            var postcode = string.Empty;
            var latitude = 0m;
            var longitude = 0m;
            if (res["status"] == "OK")
            {
                var name = res["result"]["name"];
                var types = string.Join(",", res["result"]["types"]);
                string city = string.Empty;
                string state = string.Empty;
                foreach (var addressComponent in res["result"]["address_components"])
                {
                    foreach (var t in addressComponent["types"])
                    {
                        if (t == "locality")
                        {
                            city = addressComponent["long_name"];
                        }
                        if (t == "administrative_area_level_1")
                        {
                            state = addressComponent["long_name"];
                        }
                    }
                }

                var address = HasProperty(res["result"], "vicinity") ? res["result"]["vicinity"] : string.Empty;
                var phone = HasProperty(res["result"], "international_phone_number") ? res["result"]["international_phone_number"] : string.Empty;
                var website = HasProperty(res["result"], "website") ? res["result"]["website"] : string.Empty;
                var placeid = res["result"]["place_id"];
              
             
            }
            return new Tuple<decimal, decimal, string>(latitude, longitude, postcode);
        }

        public static bool HasProperty(dynamic obj, string name)
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
