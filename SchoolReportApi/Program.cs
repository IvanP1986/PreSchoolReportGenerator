using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Configuration;

namespace SchoolReportApi
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];


            using (WebApp.Start(url: baseAddress))
            {
                Console.WriteLine($"Api started...on {baseAddress}");
                Console.ReadKey();
            }
        }
    }
}
