using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using OpenQA.Selenium.Chrome;
using Funda;
namespace ConsoleLauncher
{
    class Program
    {
        static void Main(string[] args)
        {

            //var driver =
            //      new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);


            using (var crawler = new Crawler())
            {
                crawler.Navigate("https://www.fotocasa.es/en/");
            }


        }
    }
}
