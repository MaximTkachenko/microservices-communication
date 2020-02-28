using System.Threading.Tasks;
using Common;
using Common.Health;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Portal.Db;
using Portal.Middleware;
using Portal.Services;
using Serilog;

namespace Portal
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
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.Scope.Add(OpenIdConnectScope.OfflineAccess);
                options.Scope.Add("api://theapp.api/UsersAndClaims");
                options.Scope.Add("api://theapp.api/Tickets");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Instead of using the default validation (validating against a single issuer value, as we do in
                    // line of business apps), we inject our own multitenant validation logic
                    ValidateIssuer = false,

                    // If the app is meant to be accessed by entire organizations, add your issuer validation logic here.
                    //IssuerValidator = (issuer, securityToken, validationParameters) => {
                    //    if (myIssuerValidationLogic(issuer)) return issuer;
                    //}
                };

                //https://docs.microsoft.com/en-us/azure/architecture/multitenant-identity/token-cache
                //https://joonasw.net/view/aspnet-core-2-azure-ad-authentication
                options.Events = new OpenIdConnectEvents
                {
                    OnTicketReceived = context =>
                    {
                        // If your authentication logic is based on users then add your logic here
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/Error");
                        context.HandleResponse(); // Suppress the exception
                        return Task.CompletedTask;
                    },
                    OnAuthorizationCodeReceived = async context =>
                    {
                        //todo accessTokenAcceptedVersion, scopes, and scopes for portal app
                        //https://docs.microsoft.com/en-us/azure/active-directory/develop/reference-app-manifest
                        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenAcquisitionService>();
                        await tokenService.AcquireTokenByAuthorizationCodeAsync(context);
                    }
                    // If your application needs to authenticate single users, add your user validation below.
                    //OnTokenValidated = context =>
                    //{
                    //    return myUserValidationLogic(context.Ticket.Principal);
                    //}
                };
            });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddScoped<IAccessTokenGetter, FromCacheAccessTokenGetter>();
            services.AddScoped<ITokenAcquisitionService, TokenAcquisitionService>();
            services.AddScoped<IDbTokenCache, DbTokenCache>();
            services.AddDbContext<PortalDb>(x => x.UseSqlServer(Configuration.GetConnectionString("PortalDb")));
            services.AddHealthChecks()
                .AddCheck<EnvHealthCheck>("env");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDbSeed("PortalDb");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseUserClaimsDump("/claims-dump-1");
            app.UseTokenAcquisitionException();
            app.UseRemoteClaimsHydration();
            app.UseUserClaimsDump("/claims-dump-2");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });
            });
        }
    }
}
