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
        public  void CallGooglePlacesAPIAndSetCallback()
        {
            // if (!File.Exists("results.csv")) { File.CreateText("results.csv"); }
            // var keywords = "(" + string.Join(") OR (", ConfigurationManager.AppSettings.Get("keywords").Split(new[] { ',' })) + ")";
            var googlePlacesApiKey = "AIzaSyB81BD-GxRYR3MH3fcq51AWS5m0WDr7hjM";
            //  var radius = ConfigurationManager.AppSettings.Get("radius");
            //   string filename = ConfigurationManager.AppSettings.Get("coordinateSource");
            using (var db = new Funda.WebAnalyzerEntities())
            {
                var addresseToBeSearched = db.Rent.Where<IRecord>(o => o.PostCode == null && o.Url.Contains("daft")).Union(db.Rent.Where<IRecord>(oo => oo.PostCode == null && oo.Url.Contains("daft"))).Distinct();

                foreach (var locationTobeSearched in addresseToBeSearched)
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
                                        var coordinatesAndPostCode = ReadResponse(match);
                                        if (coordinatesAndPostCode != null) {
//                                            locationTobeSearched.
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
                }
            }
        }

     

        public  Tuple<decimal,decimal,string> ReadResponse(dynamic res)
        {

            try
            {
                var latitude = (decimal)res["geometry"]["location"]["lat"];
                var longitude = (decimal)res["geometry"]["location"]["lng"];
                var postcode = (string)GetPostalCode(res);

                return new Tuple<decimal, decimal, string>(latitude, longitude, postcode);
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
                            return type["long_name"];
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
