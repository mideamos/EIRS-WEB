using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using Microsoft.Owin.Security.OAuth;
using System;
using EIRS.API.App_Start;

[assembly: OwinStartup(typeof(EIRS.API.Startup))]

namespace EIRS.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ///
            var publicClientId = "self";
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,

                //The Path For generating the Toekn  
                TokenEndpointPath = new PathString("/Token"),
                //TokenEndpointPath = new PathString("/Account/Login"),

                //Setting the Token Expired Time (24 hours)  
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                AuthorizeEndpointPath = new PathString(""),

                //AuthorizationServerProvider class will validate the user credentials  
                Provider = new ApplicationOAuthProvider(publicClientId),
            };

            //Token Generations  
            app.UseOAuthAuthorizationServer(options);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            ///////////////////////////////////////////////////////////////////////////////////////
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            app.UseWebApi(config);
            
            // Configure the HTTP request pipeline.

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint($"V1/swagger.json", "This PAYE API is created by tytunji29@gmail.com");
            //});
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;


           
        }
    }
}
