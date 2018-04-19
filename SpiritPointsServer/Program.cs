using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SpiritPointsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.DataPath = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "DataPath.txt"))[0];
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:80")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
