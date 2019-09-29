namespace TestWebAPI
{
    using System.Linq;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using TestWebApi.Data;

    using TestWebApi.Data.Repositories;

    using TestWebApi.Domain.Entities;

    using TestWebAPI.EventHandlers;
    using TestWebAPI.Library;
    using TestWebAPI.Library.HealthChecks;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="hostingEnvironment">
        /// The hosting Environment.
        /// </param>
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment)
        {
            this.Configuration = configuration;
            this.LoggerFactory = loggerFactory;
            this.HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// Gets or sets the hosting environment.
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /* To create database from startup
             using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<EmployeeDataContext>();
                context.Database.Migrate();
            } */
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Adds strict transport security header
                app.UseHsts();
            }

            app.UseHealthChecks(
                "/healthcheck",
                new HealthCheckOptions()
                    {
                        ResponseWriter = WriteResponse
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddLogging(
                builder =>
                    {
                        builder.AddConsole().AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Trace);
                    });

            var connectionString =
                "Server=(localdb)\\mssqllocaldb;Database=Test-WebApi-local;Trusted_Connection=True;MultipleActiveResultSets=true";

            // var optionsBuilder = new DbContextOptionsBuilder<EmployeeDataContext>();
            /*services.AddDbContext<EmployeeDataContext>(optionsBuilder =>
                {
                    optionsBuilder.UseSqlServer(
                        connectionString,
                        option =>
                            {
                            option.EnableRetryOnFailure();
                        });

                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseLoggerFactory(this.LoggerFactory);
            });*/

            /*
            https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#dbcontext-pooling 
            Pooling has some performance gains. Avoid using DbContext Pooling if you maintain your own state 
            (for example, private fields) in your derived DbContext class that should not be shared across requests.
            services.AddDbContextPool<EmployeeDataContext>(options => { });  */

            var logger = this.LoggerFactory.CreateLogger("Delegate");

            services.AddScoped<EmployeeDataContext>(
                sp =>
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<EmployeeDataContext>();
                        optionsBuilder.UseSqlServer(
                            connectionString,
                            option =>
                                {
                                    option.EnableRetryOnFailure();
                                });

                        optionsBuilder.EnableSensitiveDataLogging();
                        optionsBuilder.UseLoggerFactory(this.LoggerFactory);

                        var context = new EmployeeDataContext(optionsBuilder.Options);

                        context.OnSaveEventHandlers = EntityEventHandler.OnSave;
                            
                        context.OnSaveEventHandlers += (entries) =>
                            {
                                logger.LogInformation($"Delegate 2 Invoked.");
                            }; 

                        return context;
                    });

            services.AddScoped<IRepository<Employee>, GenericRepository<Employee, EmployeeDataContext>>();
            services.AddScoped<IRepository<Address>, GenericRepository<Address, EmployeeDataContext>>();

            services.AddHealthChecks().AddMemoryHealthCheck("memory");
        }

        /// <summary>
        /// The write response.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty(
                    "results",
                    new JObject(
                        result.Entries.Select(
                            pair => new JProperty(
                                pair.Key,
                                new JObject(
                                    new JProperty("status", pair.Value.Status.ToString()),
                                    new JProperty("description", pair.Value.Description),
                                    new JProperty(
                                        "data",
                                        new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}