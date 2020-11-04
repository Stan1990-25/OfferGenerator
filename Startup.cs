using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMOffersClients.Models;

namespace TMOffersClients
{
    public class Startup
    {
        #region Private members

        /// <summary>
        /// The configuration object
        /// </summary>
        private IConfiguration mConfig;

        #endregion

        #region Public properties

        public static IWebHostEnvironment Environment { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="config">The passed in configuration</param>
        public Startup(IConfiguration config)
        {
            mConfig = config;
        }

        #endregion
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // A service to use SQL server
            services.AddDbContextPool<AppDbContext>(options =>
                                                        options.UseSqlServer(mConfig.GetConnectionString("TMVarnaDBConn")));

            services.AddIdentity<TMUser, IdentityRole>(opts => {
                opts.Password.RequiredLength = 10;
                opts.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();

            //services.ConfigureApplicationCookie(opts => opts.LoginPath = "/home/index");

            services.AddMvc(options => options.EnableEndpointRouting = false)
                    .AddJsonOptions(opts => opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve)
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization();

            // DI services for the implementation classes
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IContactPersonRepository, ContactPersonRepository>();
            services.AddScoped<ICouriersRepository, CouriersRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryAddressesRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOffersRepository, OffersRepository>();          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage(); // As early as possible

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            Environment = env;
        }
    }
}
