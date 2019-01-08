using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp2.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using BlogApp2.Models;

namespace BlogApp2
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Creating a policy to allow users to post blogs via the claims.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Blogger", policy => policy.RequireClaim("Admin", "true"));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context, UserManager<IdentityUser> userManager, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Blog}/{action=JustBlogs}/{id?}");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();
            app.UseAuthentication();
            DbInitializer.Intialize(context,userManager);
            CreateClaim(serviceProvider).Wait();
        }

        //Allowing more privileges to some users via claim based authorization.
        private async Task CreateClaim(IServiceProvider serviceProvider)
        {
            var um = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //This user can post blogs.
            IdentityUser user = await um.FindByEmailAsync("Member1@email.com");

            var claimList = (await um.GetClaimsAsync(user)).Select(p => p.Type);

            if (!claimList.Contains("Admin"))
            {
                await um.AddClaimAsync(user, new Claim("Admin", "true"));
            }

        }
    }
}
