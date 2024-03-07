

using Film;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Films
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    config.SetBasePath(Directory.GetCurrentDirectory());

                    if (env == Environments.Development)
                    {
                        // Charger appsettings.Development.json indépendamment de l'environnement
                        config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true); config.AddUserSecrets<Startup>();
                    }
                    else
                    {
                        // Charger appsettings.Production.json indépendamment de l'environnement
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    }



                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}