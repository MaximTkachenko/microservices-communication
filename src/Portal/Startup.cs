using System;
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
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Portal.Db;
using Portal.Middleware;
using Portal.Services;
using Serilog;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;
using TokenCache = Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache;

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
                if (options.Authority.IsVersion2())
                {
                    options.Scope.Add("api://theapp.api/UsersAndClaims");
                    options.Scope.Add("api://theapp.api/Tickets");
                }
                else
                {
                    options.Resource = "api://theapp.api";
                }

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
                        var request = context.HttpContext.Request;
                        var currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);

                        //todo accessTokenAcceptedVersion, scopes, and scopes for portal app
                        //https://docs.microsoft.com/en-us/azure/active-directory/develop/reference-app-manifest
                        if (context.Options.Authority.IsVersion2())
                        {
                            //use MSAL for v2.0
                            //https://docs.microsoft.com/bs-latn-ba/azure/active-directory/develop/msal-net-instantiate-confidential-client-config-options
                            //https://joonasw.net/view/azure-ad-v2-and-msal-from-dev-pov
                            //https://securecloud.blog/2019/05/22/azure-api-management-jwt-validation-for-multiple-azure-ad-partner-registrations/
                            //https://thomaslevesque.com/2018/12/24/multitenant-azure-ad-issuer-validation-in-asp-net-core/
                            //https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant
                            var co = new ConfidentialClientApplicationOptions
                            {
                                Instance = "https://login.microsoftonline.com/",
                                TenantId = "common",
                                ClientId = context.Options.ClientId,
                                ClientSecret = context.Options.ClientSecret,
                                RedirectUri = currentUri
                            };
                            //todo cache tokens
                            var app = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(co)
                                .Build();
                            var result = await app.AcquireTokenByAuthorizationCode(new []{ "api://theapp.api/UsersAndClaims", "api://theapp.api/Tickets" }, context.ProtocolMessage.Code)
                                .ExecuteAsync();

                            context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                        }
                        else
                        {
                            //use ADAL for v1.0
                            var credential = new ClientCredential(context.Options.ClientId, context.Options.ClientSecret);
                            var authContext = new AuthenticationContext(context.Options.Authority, context.HttpContext.RequestServices.GetService<TokenCache>());
                            
                            var result = await authContext.AcquireTokenByAuthorizationCodeAsync(context.ProtocolMessage.Code, new Uri(currentUri), credential);

                            context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                        }
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
            services.AddHttpClient();
            services.AddSingleton<TokenCache>();//todo need to improve
            services.AddSingleton<IAccessTokenGetter, FromCacheAccessTokenGetter>();
            services.AddDbContext<PortalDb>(x => x.UseSqlServer(Configuration.GetValue<string>("Db:PortalDb")));
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
