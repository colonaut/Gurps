using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MedienKultur.Gurps.Models;
using MedienKultur.Identity.RavenDB;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    public static class ApplicationUserExtensions
    {
        private const string GravatarUrl = "https://www.gravatar.com/avatar/";
        
        public static string Gravatar(this ApplicationUser applicationUser)
        {
            var encoder = new UTF8Encoding();
            var md5 = new MD5CryptoServiceProvider();
            var hashedBytes = md5.ComputeHash(encoder
                .GetBytes(applicationUser
                    .GravatarEmail
                    .Trim()
                    .ToLowerInvariant()));
            var sb = new StringBuilder(hashedBytes.Length * 2);

            for (var i = 0; i < hashedBytes.Length; i++)
                sb.Append(hashedBytes[i].ToString("X2").ToLowerInvariant());
            
            return GravatarUrl + sb.ToString();
        }
    }

    public static class PrincipalExtensions
    {
        //public class UserStore : IUserStore<ApplicationUser>
        //{
        //    public void Dispose()
        //    {
        //        throw new System.NotImplementedException();
        //    }

        //    public Task CreateAsync(ApplicationUser user)
        //    {
        //        throw new System.NotImplementedException();
        //    }

        //    public Task UpdateAsync(ApplicationUser user)
        //    {
        //        throw new System.NotImplementedException();
        //    }

        //    public Task DeleteAsync(ApplicationUser user)
        //    {
        //        throw new System.NotImplementedException();
        //    }

        //    public Task<ApplicationUser> FindByIdAsync(string userId)
        //    {
        //        throw new System.NotImplementedException();
        //    }

        //    public Task<ApplicationUser> FindByNameAsync(string userName)
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}
        
        public static ApplicationUser ApplicationUser(this IPrincipal user)
        {
            //var userPrincipal = (GenericPrincipal)user; cann also be claim principal..
            //var userManager = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(() => IDocumentSession ));

            if (user.Identity.IsAuthenticated)
                
                return new ApplicationUser("colonaut@gmx.de"){GravatarEmail = "colonaut@gmx.de"};
                
                //return userManager.FindByName(user.Identity.GetUserName());
            
            return null;
        }
    }
    
    
    public class AuthenticationController : Controller
    {
        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [Route("login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                
                if (loginModel.HasValidUsernameAndPassword)
                {
                    var identity = new ClaimsIdentity(new [] {
                            new Claim(ClaimTypes.Name, loginModel.UserName),                            
                        },
                        DefaultAuthenticationTypes.ApplicationCookie,
                        ClaimTypes.Name,
                        ClaimTypes.Role);

                    // if you want roles, just add as many as you want here (for loop maybe?)
                    identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
                    // tell OWIN the identity provider, optional
                    // identity.AddClaim(new Claim(IdentityProvider, "Simplest Auth"));

                    Authentication.SignIn(new AuthenticationProperties
                                          {
                                              IsPersistent = loginModel.RememberMe
                                          }, identity);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Login", loginModel);
        }



        [Route("logout")]
        [HttpGet]
        public ActionResult Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }
    }
}