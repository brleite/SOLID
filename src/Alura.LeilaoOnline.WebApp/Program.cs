using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Alura.LeilaoOnline.WebApp.Seeding;
using Microsoft.AspNetCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alura.LeilaoOnline.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            //Host.CreateDefaultBuilder(args)
            //    .ConfigureWebHostDefaults(webBuilder =>
            //    {
            //        webBuilder.UseStartup<Startup>();
            //    });


        WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
