using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filters101.Filters;
using Filters101.Infrastructure.Data;
using Filters101.Interfaces;
using Filters101.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Filters101.Infrastructure.Services;

namespace Filters101
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
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase());
            // Add framework services.
            services.AddMvc(options => options.Filters.Add(new DurationActionFilter()));

            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddTransient<RandomNumberProviderFilter>();
            services.AddTransient<RandomNumberService>();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            this.Configure(app, env, loggerFactory);
            PopulateTestData(app);
            //var authorRepository = app.ApplicationServices
            //    .GetService<IAuthorRepository>();
            //Task.Run(() => PopulateSampleData(authorRepository));
        }

        private void PopulateTestData(IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<AppDbContext>();
            var authors = dbContext.Authors;
            foreach (var author in authors)
            {
                dbContext.Remove(author);
            }
            dbContext.SaveChanges();
            dbContext.Authors.Add(new Author()
            {
                Id=1,
                FullName = "Steve Smith",
                TwitterAlias = "ardalis"
            });
            dbContext.Authors.Add(new Author()
            {
                Id=2,
                FullName = "Neil Gaiman",
                TwitterAlias = "neilhimself"
            });
            dbContext.SaveChanges();
        }

        //private async Task PopulateSampleData(IAuthorRepository authorRepository)
        //{
        //    var authors = await authorRepository.ListAsync();
        //    foreach (var author in authors)
        //    {
        //        await authorRepository.DeleteAsync(author.Id);
        //    }
        //    await authorRepository.AddAsync(new Author()
        //    {
        //        FullName = "Steve Smith",
        //        TwitterAlias = "ardalis"
        //    });
        //    await authorRepository.AddAsync(new Author()
        //    {
        //        FullName = "Neil Gaiman",
        //        TwitterAlias = "neilhimself"
        //    });
        //}


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
