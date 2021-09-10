using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.DAL;

namespace TodoApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private const string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //esses metodos sao chamados pela runtime do asp.net core quando o aplicativo e inicado
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
            {
                opt.UseSqlServer(_configuration.GetConnectionString("TodoContext"));
            });
            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy(_myAllowSpecificOrigins, builder =>
                {
                    //file:///C:/Users/Pichau/source/repos/TodoAppClient/
                    builder.WithOrigins("")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //we have specified which parameters must be taken into account to consider JWT as valid
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecreetKey"]))
                    };
                });

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(_myAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
