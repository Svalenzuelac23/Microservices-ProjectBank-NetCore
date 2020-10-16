using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace MS.AFORO255.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //ACA ESTAMOS CONFIGURACION SPRING PARA CENTRALIZAR LAS CONFIGURACIONES
                    webBuilder.ConfigureAppConfiguration((host, builder) =>
                    {
                        var env = host.HostingEnvironment;
                        builder.AddConfigServer(env.EnvironmentName);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
