using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Authorization.IdentityServer.Data;


using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Authorization.IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseInMemoryDatabase("MEMORY");
            })
                .AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();




            //если надо изменить аккаунт/login... который создается то используем userInteraction
            services.AddIdentityServer(options=> 
            {
                options.UserInteraction.LoginUrl = "/Auth/Login";
            })
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryClients(Configuration.GetClients())
                .AddInMemoryIdentityResources(Configuration.GetIdentityResourses())
                .AddInMemoryApiResources(Configuration.GetApiResources())
                //.AddInMemoryApiScopes(Configuration.GetApiScopes()) // IdentityServer4 version 4.x.x changes
                .AddDeveloperSigningCredential();

               

            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
