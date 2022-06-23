using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentalAPI.DbAccessors.SortingServices;
using RentalAPI.Mapping;
using RentalAPI.Models;
using RentalAPI.Persistance;
using RentalAPI.Persistance.DbSeed;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services;
using RentalAPI.Services.Interfaces;
using System;
using System.Text;

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
            services.AddIdentity<RentalUser, IdentityRole>(options =>
                     {
                         options.Password.RequireNonAlphanumeric = false;
                     })
                    .AddEntityFrameworkStores<RentalDbContext>();
  
            services.AddAuthentication(
              options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })        
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("JWTSettings:ValidIssuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JWTSettings:SecretKey").Value)),
                        ValidAudience = Configuration.GetSection("JWTSettings:ValidAudience").Value,
                        ClockSkew = TimeSpan.Zero
                    };
                }
            );

            services.AddAuthorization(options =>
            {
                /*options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();*/
                options.AddPolicy("IsAdministrator", policy => policy.RequireRole("Administrator"));
                     
            }); 
                                     
            services.AddDbContext<RentalDbContext>(options =>
                                  options.UseSqlServer(Configuration.GetConnectionString("RentalDbConnection"))); 

            services.AddControllers().AddNewtonsoftJson()
                                    .AddFluentValidation(options =>
                                    {
                                        options.RegisterValidatorsFromAssemblyContaining<Startup>();
                                    })
                                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddOData();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToDTO());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RentalAPI", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { 
                {
                     new OpenApiSecurityScheme{
                         Reference = new OpenApiReference{
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"}
                     },
                     new string[] {}
                }});
             });    

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<IDamageRepository, DamageRepository>();
            services.AddScoped<IDamageService, DamageService>();

            services.AddScoped<IRentableRepository, RentableRepository>();
            services.AddScoped<IRentableService, RentableService>();

            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractService, ContractService>();

            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IRentalService, RentalService>();

            services.AddScoped<IVehicleRentalRepository, VehicleRentalRepository>();
            services.AddScoped<IVehicleRentalService, VehicleRentalService>();

            services.AddScoped<IDamageRepository, DamageRepository>();
            services.AddScoped<IDamageService, DamageService>();

            services.AddScoped<ICurrencyRateExchanger, CurrencyRateExchanger>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IdentitySeeder>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().Expand().Count().OrderBy();
                endpoints.MapControllers();           
            });
        }
    }
}

// to do
// check input ref values
// return the correct status codes
// return not acceptable format: Add Controllers configurare ReturnHttpNotAcceptable = true
// add extract output format support:
// add exception handler for production
// add head support
// add filter and search parameters in a separate class binded with [FromQuery]
// cand fac filter si search iau tabela asQueryable(), apoi fac search-urile cu Linq, apoi fac ToList, ToDictionary, ToArray pt executia queryurilor
// createdAtRoute for POST
// add patch action