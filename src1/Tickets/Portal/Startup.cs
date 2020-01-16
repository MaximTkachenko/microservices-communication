using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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
                options.Resource = "api://theapp.api";
                options.SaveTokens = true;

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
                    //todo how to store token?!!!
                    OnAuthorizationCodeReceived = async context =>
                    {
                        var request = context.HttpContext.Request;
                        var credential = new ClientCredential(context.Options.ClientId, context.Options.ClientSecret);
                        var authContext = new AuthenticationContext(context.Options.Authority);
                        var currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);

                        var result = await authContext.AcquireTokenByAuthorizationCodeAsync(
                            context.ProtocolMessage.Code, new Uri(currentUri), credential, context.Options.Resource);

                        /*jwt sample:
                        {
                          "typ": "JWT",
                          "alg": "RS256",
                          "x5t": "piVlloQDSMKxh1m2ygqGSVdgFpA",
                          "kid": "piVlloQDSMKxh1m2ygqGSVdgFpA"
                        }.{
                          "aud": "api://theapp.api",
                          "iss": "https://sts.windows.net/6b9be1b6-4f80-4ce7-8479-16c4d7726470/",
                          "iat": 1579196108,
                          "nbf": 1579196108,
                          "exp": 1579200008,
                          "acr": "1",
                          "aio": "AVQAq/8OAAAAHLg2UT5qfZ230dYPJdzk14ooexDdZowHBfKshArz7hAc1CVrWZQ1VzjPmk1eT6Os1+wC7zGXf32LiPCWKJ+as63NbWZ9CoqCneXhNWbcRtY=",
                          "amr": [
                            "pwd"
                          ],
                          "appid": "b021b14e-1671-4fe6-b7cc-0a67a248543f",
                          "appidacr": "1",
                          "email": "oblomov86@gmail.com",
                          "family_name": "Tkachenko",
                          "given_name": "Maxim",
                          "idp": "live.com",
                          "ipaddr": "51.174.85.2",
                          "name": "Maxim Tkachenko",
                          "oid": "03526494-16e1-4e21-99a5-9d734186092e",
                          "scp": "Tickets UsersAndClaims",
                          "sub": "hI_OiH4kmvVkzY_NU24aOlahR06Dul7zZe5smXJHM90",
                          "tid": "6b9be1b6-4f80-4ce7-8479-16c4d7726470",
                          "unique_name": "live.com#oblomov86@gmail.com",
                          "uti": "IMYPesSovk2YXZAm5Og9AQ",
                          "ver": "1.0"
                        }.[Signature]
                         */
                        context.HandleCodeRedemption(result.AccessToken, result.IdToken);
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
