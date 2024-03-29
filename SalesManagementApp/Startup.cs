﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Account;
using Account.Roles;
using Account.Users;
using Database;
using Database.Entities;
using Farmers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.DataServiceLayer;
using Order.IRepositories;
using Order.Repositories;
using Purechase;
using Safes;
using Salesinvoice;
using Sellers;
using Shared.IRepository;

namespace SalesManagementApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromHours(1);//You can set Time   
            });

            services.AddMvc();
            //Set Session Timeout. Default is 20 minutes.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(300);
            });
            // Add the whole configuration object here.
            services.AddSingleton<IConfiguration>(Configuration);


            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            //Increase Form Count Limit
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 4096; // 200 items max
                options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
            });

            services.AddDbContext<EntitiesDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IFarmerOperationsRepo, FarmerOperationsRepo>();
            services.AddTransient<ISellerOperationsRepo, OrderHeaderRepository>();
            services.AddTransient<IOrderHeaderOperationsRepo, OrderHeaderOperationsRepo>();
            services.AddTransient<IOrderDetailsOperationsRepo, OrderDetailsOperationsRepo>();
            services.AddTransient<IPurechasesOperationsRepo, PurechasesOperationsRepo>();
            services.AddTransient<ISalesinvoicesOperationsRepo, SalesinvoicesOperationsRepo>();
            services.AddTransient<IOrderOperationsDSL, OrderOperationsDSL>();
            services.AddTransient<IOrder_PurechaseOperationsRepo, Order_PurechaseOperationsRepo>();
            services.AddTransient<IUserOperationsRepo, UserOperationsRepo>();
            services.AddTransient<IAccountRepo, AccountRepo>();
            services.AddTransient<ISafeOperationsRepo, SafeOperationsRepo>();

            services.AddTransient<IRoleOperationsRepo, RoleOperationsRepo>();

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                   template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
