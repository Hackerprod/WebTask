using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTask.Database;
using WebTask.Entities;
using WebTask.Models;
using WebTask.Services;

namespace WebTask
{
    public class Startup
    {
        public static string SecretKey => "ijurkbdlhmklqacwqzdxmkkhvqowlyqa";
        private string ConnectionString = "server=localhost;port=3306;database=TaskDB;user=root;password=loops";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebTask", Version = "v1" });
            });

            services.AddDbContextPool<DBContext>(builder => { builder.UseMySQL(Configuration.GetConnectionString("DefaultConnection")); });

            var key = Encoding.ASCII.GetBytes(SecretKey);
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
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserService, UserService>();
        }

        private void fgf(DbContextOptionsBuilder obj)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DBContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebTask"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            context.Database.EnsureCreated();

            PrepareDB(context);
        }
        private void ConfigureJSON1(JsonOptions obj)
        {
            //obj.JsonSerializerOptions.
        }
        //private void ConfigureJSON(MvcJsonOptions obj)
        //{
        //    obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //}
        private void PrepareDB(DBContext context)
        {
            User u1 = new User { Id = 1000, FirstName = "Daimel", LastName = "Rodriguez Torres", Email = "hackerprodlive@gmail.com", Username = "Hackerprod", Password = "loops", Role = Role.Admin };
            User u2 = new User { Id = 1001, FirstName = "Dayron", LastName = "Alfaro Gonzales", Email = "Jst4rk@gmail.com", Username = "Jhon", Password = "jhon", Role = Role.Seller };
            User u3 = new User { Id = 1002, FirstName = "Julio", LastName = "Garcia Hernandez", Email = "stickm4n@gmail.com", Username = "Stick", Password = "stick", Role = Role.Client };

            if (!context.Users.Any())
            {
                context.Users.Add(u1);
                context.Users.Add(u2);
                context.Users.Add(u3);
            }


            Product p1 = new Product { ProductID = 1, Slug = Extensions.GetUniqueAlphaNumericID(), Name = "Mouse", CreatedByUser = 1000, Description = "Mouse, Color Red", Price = 12 };
            Product p2 = new Product { ProductID = 2, Slug = Extensions.GetUniqueAlphaNumericID(), Name = "Mouse", CreatedByUser = 1000, Description = "Mouse, Color Blue", Price = 18 };
            Product p3 = new Product { ProductID = 3, Slug = Extensions.GetUniqueAlphaNumericID(), Name = "Motherboard", CreatedByUser = 1001, Description = "Asus ROG Maximus Hero VI", Price = 251 };
            Product p4 = new Product { ProductID = 4, Slug = Extensions.GetUniqueAlphaNumericID(), Name = "Keyboard", CreatedByUser = 1002, Description = "Gamer keyboard", Price = 10 };

            ProductCard pc1 = new ProductCard() { Product = p1, Quantity = 3 };
            ProductCard pc2 = new ProductCard() { Product = p2, Quantity = 1 };
            ProductCard pc3 = new ProductCard() { Product = p3, Quantity = 2 };
            ProductCard pc4 = new ProductCard() { Product = p4, Quantity = 10 };

            if (!context.Products.Any())
            {
                context.Products.Add(pc1);
                context.Products.Add(pc2);
                context.Products.Add(pc3);
                context.Products.Add(pc4);
            }
            

            Order o1 = new Order { OrderId = 1, Quantity = 1, State = OrdenStatus.Created, OrderUserId = u2.Id, Product = p1, DateCreated = new DateTime(2019, 4, 26) };
            Order o2 = new Order { OrderId = 2, Quantity = 4, State = OrdenStatus.Created, OrderUserId = u3.Id, Product = p1, DateCreated = new DateTime(2020, 4, 21) };

            if (!context.Orders.Any())
            {
                context.Orders.Add(o1);
                context.Orders.Add(o2);
            }

            context.SaveChanges();

        }

    }
}
