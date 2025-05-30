using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BookStoreApp.filters;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Helpers;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace BookStoreApp
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
            services.AddControllers();

            //for configure database connection
            services.AddDbContext<BookDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DbConnection"]));
            //services.AddDbContext<BooksContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DbConnection"]));

            // For Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisConnection");
            });

            //for user
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IUserManager, UserManager>();

            //for token
            services.AddTransient<JwtTokenManager>();
            services.AddTransient<Send>();


            //for user
            services.AddTransient<IAdminRepo, AdminRepo>();
            services.AddTransient<IAdminManager, AdminManager>();

            //for books
            services.AddTransient<IBooksManager, BooksManager>();
            services.AddTransient<IBooksRepo, BooksRepo>();

            //for carts
            services.AddTransient<ICartManager, CartManager>();
            services.AddTransient<ICartRepo, CartRepo>();

            //for wishlist
            services.AddTransient<IWishlistsManager, WishlistsManager>();
            services.AddTransient<IWishlistsRepo, WishlistsRepo>();

            //for orders
            services.AddTransient<IOrdersManager, OrdersManager>();
            services.AddTransient<IOrdersRepo, OrdersRepo>();

            //for customerDetails
            services.AddTransient<ICustomerDetailsManager, CustomerDetailsManager>();
            services.AddTransient<ICustomerDetailsRepo, CustomerDetailsRepo>();


            //For swagger
            //services.AddSwaggerGen();

            //for adding Authorization in swagger --- to paste token
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Book-Store-App API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter you valid Token",
                });
                
                c.OperationFilter<AuthorizeOptionFilter>();

            });


            //Configuring MassTrasit --- RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });

            services.AddMassTransitHostedService();

            //for jwt token
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // to handle missing/invalid token globally
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse(); // Prevent default 401 response

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            success = false,
                            message = "Authorization token is missing or invalid! Please login to access this resource."
                        });

                        return context.Response.WriteAsync(result);
                    },

                    // 403 - Forbidden (valid token, but not Admin)
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new ResponseModel<string>
                        {
                            Success = false,
                            Message = "Access denied! You do not have the authorization to perform this action.",
                            Data = null
                        });

                        return context.Response.WriteAsync(result);
                    }
                };





            });

            //This tells ASP.NET Core to use Newtonsoft.Json
            //instead of the default System.Text.Json for JSON serialization/deserialization.
            // to avoids infinite loops during serialization
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });


            //for injecting IHttpContextAccessor
            //used to access the current HTTP context (request info, headers, user claims, etc.) 
            services.AddHttpContextAccessor();

            //CORS
            // Named Policy
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            //for handle DoS attack
            //IMemoryCache
            services.AddMemoryCache();

            // global rate limit: 10 req/60 sec
            services.AddControllers(options =>
            {
                options.Filters.Add(new RateLimitAttribute(10, 60)); 
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //On top -CORS
            app.UseCors("AllowOrigin");

            //authentication must be on top
            app.UseAuthentication();
            app.UseAuthorization(); // this is the key

            // This middleware serves generated Swagger document as a JSON endpoint
            app.UseSwagger();

            //This middleware serves the Swagger documentation UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Stores API V1");
            });


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
