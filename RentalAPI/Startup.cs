using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RentalAPI.Mapping;
using RentalAPI.Persistance;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services;
using RentalAPI.Services.Interfaces;
using RentalAPI.ValidationFilters;

namespace RentalAPI
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
            services.AddMvc(options =>
                           {
                                options.EnableEndpointRouting = false;
                                options.Filters.Add(new ValidationFilter());
                           })
                          .AddFluentValidation(options => 
                           { 
                               options.RegisterValidatorsFromAssemblyContaining<Startup>(); 
                           })
                          .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                         
            services.AddDbContext<RentalDbContext>(options =>
                                  options.UseSqlServer(Configuration.GetConnectionString("RentalDbConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
          
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, ClientService>();
            
            services.AddScoped<IDamageRepository, DamageRepository>();
            services.AddScoped<IDamageService, DamageService>();

            services.AddScoped<IRentableRepository, RentableRepository>();
            services.AddScoped<IRentableService, RentableService>();
     
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractService, ContractService>();
           
            services.AddScoped<IVehicleRentalRepository, VehicleRentalRepository>();
            services.AddScoped<IVehicleRentalService, VehicleRentalService>();

            services.AddScoped<IRentalDamageRepository, RentalDamageRepository>();
            services.AddScoped<IRentalDamageService, RentalDamageService>();

            services.AddScoped<ICurrencyRateExchanger, CurrencyRateExchanger>();

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToDTO());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentalAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentalAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
