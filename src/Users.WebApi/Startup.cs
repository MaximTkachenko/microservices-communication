using Common;
using Common.Health;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Users.WebApi.AuthorizationHandlers;
using Users.WebApi.Db;
using Users.WebApi.Middlewares;

namespace Users.WebApi
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://login.microsoftonline.com/6b9be1b6-4f80-4ce7-8479-16c4d7726470/";
                    options.Audience = "api://theapp.api";
                });

            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSingleton<IAuthorizationHandler, ClaimsAuthorizationHandler>();
            services.AddHttpClient();
            services.AddDbContext<UsersDb>(x => x.UseSqlServer(Configuration.GetValue<string>("Db:UsersDb")));
            services.AddHealthChecks()
                .AddCheck<EnvHealthCheck>("env");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseUserClaimsDump("/claims-dump-1");
            app.UseLocalClaimsHydration();
            app.UseUserClaimsDump("/claims-dump-2");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });
            });
        }
    }
}
