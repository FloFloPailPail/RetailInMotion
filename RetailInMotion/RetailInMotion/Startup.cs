using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RetailInMotionBLL;
using RetailInMotionContracts.BLL;
using RetailInMotionContracts.DAL;
using RetailInMotionDAL;
using RetailInMotionDAL.Repositories;

namespace RetailInMotion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); 
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton(Configuration);
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddSingleton<IOrdersRepository>(new OrdersRepository(connectionString));
            services.AddSingleton<IOrderItemsRepository>(new OrderItemsRepository(connectionString));
            //services.AddMemoryCache
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseMvc();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
