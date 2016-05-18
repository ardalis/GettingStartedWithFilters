using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filters101.Infrastructure.Data;
using Filters101.Interfaces;
using Filters101.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddMvc();

            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            this.Configure(app, env, loggerFactory);
            var authorRepository = app.ApplicationServices
                .GetService<IAuthorRepository>();
            PopulateSampleData(authorRepository);
        }

        private void PopulateSampleData(IAuthorRepository authorRepository)
        {
            var authors = authorRepository.List();
            foreach (var author in authors)
            {
                authorRepository.Delete(author.Id);
            }
            authorRepository.Add(new Author()
            {
                FullName = "Steve Smith",
                TwitterAlias = "ardalis"
            });
            authorRepository.Add(new Author()
            {
                FullName = "Neil Gaiman",
                TwitterAlias = "neilhimself"
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
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
