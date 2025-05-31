using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace BookStoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //To get Log path here
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(option =>
                {
                    option.ClearProviders();
                    option.SetMinimumLevel(LogLevel.Trace);
                }).UseNLog();


        //Trace Gives info:
        //Trace | Debug  | Info | warning | Error | Fetal
    }
}
