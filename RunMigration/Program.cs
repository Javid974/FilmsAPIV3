using FilmsDatabase;

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDBMigrations;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;
using static System.Data.Entity.Migrations.Model.UpdateDatabaseOperation;
using Version = MongoDBMigrations.Version;

namespace RunMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            // Récupérer la section "MongoDB" de la configuration
            var mongoDbConfig = configuration.GetSection("MongoDB");
            var connectionString = mongoDbConfig.GetValue<string>("ConnectionStrings");
            var databaseName = mongoDbConfig.GetValue<string>("Database");
            // Récupérer la valeur de la clé "ConnectionStrings"
            Type type = typeof(InitialMigration);

            // Récupérer la valeur de la clé "Database"
            var runner = new MigrationEngine()
                .UseDatabase(connectionString, databaseName)
                .UseAssembly(Assembly.GetAssembly(type))
                .UseSchemeValidation(false);
            runner.Run(new Version("1.0.0"));
            runner.Run(new Version("1.0.1"));
            runner.Run(new Version("1.0.2"));

        }
    }
}
