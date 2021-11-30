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

namespace Filters101
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("db"));
            // Add framework services.
            services.AddControllers(options => options.Filters.Add(new DurationActionFilter()));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddTransient<RandomNumberProviderFilter>();
            services.AddTransient<RandomNumberService>();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env)
        {
            this.Configure(app, env);
            PopulateTestData(app);
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
