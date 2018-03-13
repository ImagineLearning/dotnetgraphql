using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StarWars.Core.Data;
using StarWars.Api.Models;
using StarWars.Data.EntityFramework;
using StarWars.Data.EntityFramework.Seed;
using Microsoft.EntityFrameworkCore;
using StarWars.Data.EntityFramework.Repositories;
using GraphQL.Types;
using GraphQL;
using StarWars.Core.Logic;
using Nest;
using System;

namespace StarWars.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Env = env;
        }

        public IConfigurationRoot Configuration { get; }
        private IHostingEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<StarWarsQuery>();
			services.AddSingleton<IElasticClient>(c => {
				var node = new Uri("http://elasticsearch.elasticsearch.svc.cluster.local:9200");
				var settings = new ConnectionSettings(node);
				settings = settings.DefaultIndex("starwars");
				return new ElasticClient(settings);
			});
            services.AddTransient<ICharacterRepository, CharacterRepository>();
            services.AddTransient<IDroidRepository, DroidRepository>();
            services.AddTransient<IHumanRepository, HumanRepository>();
            services.AddTransient<IEpisodeRepository, EpisodeRepository>();
            services.AddDbContext<StarWarsContext>(options =>
                options.UseInMemoryDatabase(databaseName: "StarWars"));
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<ITrilogyHeroes, TrilogyHeroes>();
            services.AddTransient<DroidType>();
            services.AddTransient<HumanType>();
            services.AddTransient<CharacterInterface>();
            services.AddTransient<EpisodeEnum>();
            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new StarWarsSchema(type => (GraphType) sp.GetService(type)) {Query = sp.GetService<StarWarsQuery>()});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory, StarWarsContext db)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseMvc();

            db.EnsureSeedData();
        }
    }
}
