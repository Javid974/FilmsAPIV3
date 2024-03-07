using Microsoft.OpenApi.Models;
using Services.Interfaces;
using Services;
using Repository;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Repository.Interface;
using Services.Helpers;
using NLog.Extensions.Logging;
using NLog;

namespace Film
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            LogManager.Setup().LoadConfigurationFromFile("nlog.config");
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddControllers();

            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mon API", Version = "v1" });
            });

            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<ITopMovieService, TopMovieService>();
            services.AddScoped<ITopMovieRepository, TopMovieRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IDirectorService, DirectorService>();
            services.AddScoped<IMongoDbFactory, MongoDbFactory>();
            services.AddScoped<ExcelFileParser>();
            services.AddHttpClient<TMDbService>();
            services.AddScoped<TMDbService>();
            services.Configure<ApiSettings>(_configuration.GetSection("TMDbApi"));
            services.AddSingleton<IMongoClient>(new MongoClient(_configuration.GetSection("MongoDB:ConnectionStrings").Value));
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mon API v1");
            });


        }
    }
}
