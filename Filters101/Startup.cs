using Filters101.Filters;
using Filters101.Infrastructure.Data;
using Filters101.Interfaces;
using Filters101.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Filters101.Infrastructure.Services;
using Swashbuckle.AspNetCore.Swagger;

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
                options.UseInMemoryDatabase("db"));
            // Add framework services.
            services.AddMvc(options => options.Filters.Add(new DurationActionFilter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddTransient<RandomNumberProviderFilter>();
            services.AddTransient<RandomNumberService>();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env)
        {
            this.Configure(app, env);
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
