using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Example.Api.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Example.Api.Helper;
using Example.Api.Models;
using Example.Api.Services;

namespace Example.Api
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

            //Auto-mapper to map DTO to entity classes
            services.AddAutoMapper(typeof(Startup));

            // Register the IConfiguration instance which AppSettingsModel binds to.
            services.Configure<AppSettingsModel>(Configuration);
            services.AddSingleton(Configuration.Get<AppSettingsModel>());
            services.AddResponseCaching();
            services.AddControllers(options =>
                                    //Add exception filter for known exception types
                                    options.Filters.Add(new HttpResponseExceptionFilter())) ;
                         

            var conf = Configuration.Get<AppSettingsModel>();
            // Database context DI
            services.AddScoped<Db.Interfaces.IDbContext>(provider => new Db.DbContext(conf.ConnectionStrings.DefaultConnection));

            //Add cors support
            services.AddCors();

            //Add nlog support
            services.AddLogging(logging =>
            {
                //logging.AddNLog().ConfigureNLog();
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                logging.AddConsole();
                logging.AddDebug();
                logging.AddNLog();
            });

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(conf.AppSettings.TokenAuthenticationConfig.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = conf.AppSettings.TokenAuthenticationConfig.Audience,
                    ValidIssuer = conf.AppSettings.TokenAuthenticationConfig.Issuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5), //5 minute tolerance for the expiration date
                };
            });



            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExample1CollectionService, Example1CollectionService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireClaim("Role", "Admin"));//if role based authorization is needed, add required policies
            });


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.IgnoreObsoleteActions();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tingstad Sensor API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                // c.SerializeAsV2 = true;
            });


            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Example API V1");
                c.RoutePrefix = "api";//string.Empty;
            });


            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
