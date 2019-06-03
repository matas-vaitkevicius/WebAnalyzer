using Funda;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System;
using System.Data.Entity;

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

        //huelva
        //Algeciras
        //Estepona
        //Marbella
        //Málaga
        //Motril
        //Roquetas de Mar
        //Almería
        //Aguilas
        //Los Alcázares
        //Torrevieja
        //Alicante
        //Benidorm
        //Dénia
        //Gandia
        //Cullera
        //Valencia
        public List<Funda.Crawler.FotoCasaSearch> FotoCasaSearchList()
        {
            return new List<Crawler.FotoCasaSearch> {
                 new Crawler.FotoCasaSearch {Text="alquiler/viviendas/el-puerto-de-santa-maria/todas-las-zonas", LatitudeAndLongitude="latitude=36.6014&longitude=-6.22775", PriceMin=100, PriceMax=1500, CombinedLocationIds="724,1,11,282,506,11027,0,0,0", IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/huelva-provincia/todas-las-zonas", LatitudeAndLongitude = "latitude=37.2564&longitude=-6.94977", PriceMin = 100, PriceMax = 1500, CombinedLocationIds = "724,1,21,0,0,0,0,0,0", IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/algeciras/todas-las-zonas", LatitudeAndLongitude = "latitude=36.1327&longitude=-5.44636", PriceMin = 100, PriceMax = 1500, CombinedLocationIds = "724,1,11,280,501,11004,0,0,0", IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/estepona/todas-las-zonas", PriceMin = 100, PriceMax = 1500,  IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/marbella/todas-las-zonas", LatitudeAndLongitude = "latitude=36.511&longitude=-4.8827", PriceMin = 100, PriceMax = 1500, CombinedLocationIds = "724,1,29,320,551,29069,0,0,0", IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/malaga-provincia/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/motril/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/roquetas-de-mar/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/almeria-provincia/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/aguilas/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/los-alcazares/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/torrevieja/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/alicante-provincia/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/benidorm/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/denia/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/gandia/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                                  new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/daimus/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/cullera/todas-las-zonas", PriceMin = 100, PriceMax = 1500, IsSale = false },
                 new Crawler.FotoCasaSearch { Text = "alquiler/viviendas/valencia-provincia/todas-las-zonas", LatitudeAndLongitude = "latitude=39.4699&longitude=-0.375811", PriceMin = 100, PriceMax = 1500, CombinedLocationIds ="724,19,46,0,0,0,0,0,0" , IsSale = false },


                 new Crawler.FotoCasaSearch {Text="comprar/viviendas/el-puerto-de-santa-maria/todas-las-zonas",  PriceMax=150000,  IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/huelva-provincia/todas-las-zonas",  PriceMax = 150000, CombinedLocationIds = "724,1,21,0,0,0,0,0,0", IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/algeciras/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/estepona/todas-las-zonas",  PriceMax = 150000,  IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/marbella/todas-las-zonas",   PriceMax = 150000, CombinedLocationIds = "724,1,29,320,551,29069,0,0,0", IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/malaga-provincia/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/motril/todas-las-zonas", PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/roquetas-de-mar/todas-las-zonas", PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/almeria-provincia/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/aguilas/todas-las-zonas", PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/los-alcazares/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/torrevieja/todas-las-zonas", PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/alicante-provincia/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/benidorm/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/denia/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/gandia/todas-las-zonas", PriceMax = 150000, IsSale = true },
                                  new Crawler.FotoCasaSearch { Text = "comprar/viviendas/daimus/todas-las-zonas", PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/cullera/todas-las-zonas",  PriceMax = 150000, IsSale = true },
                 new Crawler.FotoCasaSearch { Text = "comprar/viviendas/valencia-provincia/todas-las-zonas", LatitudeAndLongitude = "latitude=39.4699&longitude=-0.375811", PriceMax = 150000, CombinedLocationIds ="724,19,46,0,0,0,0,0,0" , IsSale = true },

            };
    }
        public List<Funda.Crawler.PisosSearch> PisosSearchList()
        {
            return new List<Crawler.PisosSearch> {
                 new Crawler.PisosSearch { Text = "alquiler/pisos-el_puerto_de_santa_maria",PriceMax=1500, IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-huelva",PriceMax=1500, IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-algeciras",PriceMax=1500, IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-estepona", PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-marbella",PriceMax = 1500,  IsSale = false  },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-malaga",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-motril",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-roquetas_de_mar",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-almeria",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-aguilas",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-los_alcazares",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-torrevieja",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-alicante",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-benidorm",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-denia",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-gandia",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-cullera",PriceMax = 1500,  IsSale = false },
                 new Crawler.PisosSearch { Text = "alquiler/pisos-valencia",PriceMax =1500,  IsSale = false },


                 new Crawler.PisosSearch { Text = "venta/pisos-el_puerto_de_santa_maria",PriceMax=120000, IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-huelva",PriceMax=120000, IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-algeciras",PriceMax=120000, IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-estepona", PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-marbella",PriceMax = 120000,  IsSale = true  },
                 new Crawler.PisosSearch { Text = "venta/pisos-malaga",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-motril",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-roquetas_de_mar",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-almeria",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-aguilas",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-los_alcazares",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-torrevieja",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-alicante",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-benidorm",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-denia",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-gandia",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-cullera",PriceMax = 120000,  IsSale = true },
                 new Crawler.PisosSearch { Text = "venta/pisos-valencia",PriceMax =120000,  IsSale = true },

            };
        }
        public List<Funda.Crawler.MestoUaSearch> MestoUaSearchList()
        {
            return new List<Crawler.MestoUaSearch> {
                 new Crawler.MestoUaSearch {Text="odesskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="vinnitskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="volyinskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="dnepropetrovskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="donetskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="zhitomirskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="zakarpatskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="ivano-frankovskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="kievskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="kirovogradskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="luganskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="lvovskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="nikolaevskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="poltavskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="respublika-kryim", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="rovenskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="sumskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="ternopolskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="harkovskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="hersonskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="hmelnitskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="cherkasskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="chernigovskaya-oblast", PriceMin=5000, IsSale = true },
                 new Crawler.MestoUaSearch {Text="chernovitskaya-oblast", PriceMin=5000, IsSale = true },
            };
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
                        for (int i = 0; i < 15; i++)
                        {
                            search.PaginationNumber = i;
                            crawler.Navigate(search);
                            var adverts = Enumerable.Empty<IRecord>();
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

        public void DoUpdate(string systemName, bool? isSale)
        {
            using (var crawler = new Crawler())
            {
                _lock = new object();
                lock (_lock)
                {
                    using (var db = new Funda.WebAnalyzerEntities())
                    {
                        var now = DateTime.Now;
                        var list = new List<IRecord>();
                        if (!isSale.HasValue)
                        {
                            list = db.Rent.Where<IRecord>(o => (!o.DateLastProcessed.HasValue || o.DateLastProcessed.Value < DateTime.Today) && o.DateRemoved == null && o.Url.Contains(systemName)).ToList().Union(db.Sale.Where<IRecord>(o => o.DateRemoved == null && o.Url.Contains(systemName)).ToList()).OrderBy(o => o.DateLastProcessed).ToList();
                        }
                        else
                        {
                            if (isSale.Value)
                            {
                                list = db.Sale.Where<IRecord>(o => (!o.DateLastProcessed.HasValue || o.DateLastProcessed.Value < DateTime.Today) && o.DateRemoved == null && o.Url.Contains(systemName)).ToList();
                            }
                            else
                            {
                                list = db.Rent.Where<IRecord>(o => (!o.DateLastProcessed.HasValue || o.DateLastProcessed.Value < DateTime.Today) && o.DateRemoved == null && o.Url.Contains(systemName)).ToList();
                            }
                        }

                        foreach (var rent in list)
                        {
                            try
                            {
                                crawler.Navigate(rent.Url);
                                if (systemName == "funda")
                                {
                                    crawler.GetRecordDataFromItsPage(rent);
                                }
                                else if (systemName == "fotocasa")
                                {
                                    crawler.GetRecordDataFromFotoCasa(rent);
                                }
                                else
                                {
                                    crawler.GetRecordDataFromItsPageLt(rent);
                                }
                                db.SaveChanges();

                            }
                            catch { }
                        }
                    }
                }
            }
        }

        public ActionResult UpdateExisting(bool? isSale)
        {
            DoUpdate("funda", isSale);

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
                            try
                            {
                                search.PaginationNumber = i;
                                crawler.Navigate(search);
                                var adverts = Enumerable.Empty<IRecord>();
                                if (search.IsSale)
                                {
                                    adverts = crawler.AddNewLtSales((Crawler.AruodasSearch)search).Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
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
                            catch
                            {
                            }
                        }
                   }
               }
          }

           return RedirectToAction("UpdateExistingLt");
        }

        public ActionResult UpdateExistingLt(bool? isSale)
        {
            DoUpdate("aruodas", isSale);
            return View("Index");
        }

        public ActionResult CollectNewMestoUe()
        {
            using (var crawler = new Funda.Crawler())
            {
                using (var db = new Funda.WebAnalyzerEntities())
                {
                    foreach (var search in MestoUaSearchList())
                    {
                        // SetMinMax(search);
                        for (int i = 1; i < 15; i++)
                        {
                            try
                            {
                                search.PaginationNumber = i;
                                crawler.Navigate(search);
                                var adverts = Enumerable.Empty<IRecord>();
                                if (search.IsSale)
                                {
                                    adverts = crawler.AddNewMestoUeSales((Crawler.MestoUaSearch)search).Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
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
                            catch
                            {
                            }
                        }
                    }
                }
            }

            return RedirectToAction("UpdateExistingLt");
        }

        public ActionResult UpdateExistingMestoUe(bool? isSale)
        {
            DoUpdate("aruodas", isSale);
            return View("Index");
        }

        public ActionResult CollectNewEs()
        {
            using (var crawler = new Funda.Crawler())
            {
                using (var db = new Funda.WebAnalyzerEntities())
                {
                    //foreach (var search in FotoCasaSearchList())
                    //{
                    //    // SetMinMax(search);
                    //    for (int i = 1; i < 5; i++)
                    //    {
                    //        try
                    //        {
                    //            search.PaginationNumber = i;
                    //            crawler.Navigate(search);
                    //            var adverts = Enumerable.Empty<IRecord>();
                    //            if (search.IsSale)
                    //            {
                    //                adverts = crawler.AddFotoCasaSales().Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
                    //                db.Sale.AddRange(adverts.Cast<Sale>().ToList());
                    //            }
                    //            else
                    //            {
                    //                adverts = crawler.AddFotoCasaRents().Where(o => o.Price != null).ExceptWhere(db.Rent, o => o.Url);
                    //                db.Rent.AddRange(adverts.Cast<Rent>().ToList());
                    //            }

                    //            if (!adverts.Any())
                    //            {
                    //                break;
                    //            }

                    //            db.SaveChanges();
                    //        }
                    //        catch
                    //        {
                    //        }
                    //    }
                    //}
                    foreach (var search in PisosSearchList())
                    {
                        // SetMinMax(search);
                        for (int i = 1; i < 3; i++)
                        {
                            try
                            {
                                search.PaginationNumber = i;
                                crawler.Navigate(search);

                                var adverts = Enumerable.Empty<IRecord>();
                                if (search.IsSale)
                                {
                                    adverts = crawler.AddPisosSales().Where(o => o.Price != null).ExceptWhere(db.Sale, o => o.Url);
                                    db.Sale.AddRange(adverts.Cast<Sale>().ToList());
                                }
                                else
                                {
                                    adverts = crawler.AddPisosRents().Where(o => o.Price != null).ExceptWhere(db.Rent, o => o.Url);

                                    db.Rent.AddRange(adverts.Cast<Rent>().ToList());
                                }

                                if (!adverts.Any())
                                {
                                    break;
                                }

                                db.SaveChanges();
                            }
                            catch
                            {
                            }
                        }
                    }

                }
            }

            return RedirectToAction("UpdateFotoCasa");
        }

        public ActionResult UpdateFotoCasa()
        {
            DoUpdate("fotocasa", null);
            return View("Index");
        }
    }
}