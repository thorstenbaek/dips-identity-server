using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ids
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddTestUsers(Config.Users)
                .AddCustomAuthorizeRequestValidator<SofCustomAuthorizeRequestValidator>()
                .AddCustomTokenRequestValidator<SofTokenRequestValidator>()                                
                .AddDeveloperSigningCredential();

            services.AddSingleton<ICorsPolicyService>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    //AllowedOrigins = { "https://foo", "https://bar" },
                    AllowAll = true
                };
            });

            services.AddSingleton<IContextStore, ContextStore>();
            services.AddControllersWithViews();
            services.Replace(new ServiceDescriptor(typeof(IAuthorizeResponseGenerator), typeof(SofAuthorizeResponseGenerator), ServiceLifetime.Singleton));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
            
        }
    }
}
