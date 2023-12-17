namespace Medicina
{
    using Medicina.Context;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System;
    using Microsoft.AspNetCore.Identity;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });

            });
            //services.AddIdentity<PersonContext, IdentityRole>().AddEntityFrameworkStores<PersonContext>().AddDefaultTokenProviders();

            services.AddControllers();

            services.AddDbContext<UserContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
            services.AddDbContext<PersonContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
            services.AddDbContext<LogInContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "localhost",
                    ValidAudience = "localhost",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwtConfig:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddDbContext<EquipmentContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
            services.AddDbContext<CompanyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
            services.AddDbContext<CompanyRateContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
        }




        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowOrigin");  // Place this before app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

}
