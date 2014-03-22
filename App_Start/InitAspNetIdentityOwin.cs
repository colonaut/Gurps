using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;

//TODO: the owin stuff could be an own nuget... only exposing the startup, default would be cookies....

[assembly: OwinStartup(typeof(MedienKultur.Gurps.InitAspNetIdentityOwin))]

namespace MedienKultur.Gurps
{
    public class InitAspNetIdentityOwin
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/login") //The given route for the login page
            });
        }
    }
}