using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.RavenDB;
using MedienKultur.Gurps.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Raven.Client;
using StructureMap;

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
        public static ApplicationUser GetApplicationUser(this IIdentity identity)
        {
            ApplicationUser applicationUser = new ApplicationUser("unknown"); //TODO: currently we need this in order to not break the view if you are logged in but cannot get an application user.... this should be removed once account and authentication flow is working

            using (var session = ObjectFactory.GetInstance<IDocumentSession>())
            {
                // Operations against ravenSession
                var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(session));

                if (identity.IsAuthenticated)
                    applicationUser = userManager.FindById(identity.GetUserId());
            }

            return applicationUser;
        }

    }

    //TODO ? do i want this approach?
    public enum Roles
    {
        Registered,
        Player,
        GameMaster,
        Administrator
    }

    public class AuthenticationController : Controller
    {
        readonly IDocumentSession _ravenSession;

        public AuthenticationController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;            
        }
       
        IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        UserManager<ApplicationUser> ApplicationUserManager
        {
            get
            {
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ravenSession));
                userManager.UserValidator =
                    new UserValidator<ApplicationUser>(userManager)
                    {
                        AllowOnlyAlphanumericUserNames = false
                    };
                return userManager;
            }
        }
            

        [Route("register")]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser;
                if (!string.IsNullOrWhiteSpace(registerModel.UserName))
                    applicationUser =
                        new ApplicationUser(registerModel.Email,
                            registerModel.UserName);
                else
                    applicationUser =
                        new ApplicationUser(registerModel.Email);

                var identityResult =
                    ApplicationUserManager.Create(applicationUser,
                        registerModel.Password);

                if (identityResult.Succeeded)
                {
                    ApplicationUserManager.AddToRole(applicationUser.Id,
                        Roles.Registered.ToString());

                    var claimsIdentity =
                        ApplicationUserManager.CreateIdentity(applicationUser,
                            DefaultAuthenticationTypes.ApplicationCookie);

                    var authenticationProperties =
                        new AuthenticationProperties() {};

                    AuthenticationManager.SignIn(authenticationProperties, claimsIdentity);

                    return RedirectToAction("Index", "AccountSettings");
                }
            }

            return View("Register", registerModel);
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

                    AuthenticationManager.SignIn(new AuthenticationProperties
                                          {
                                              IsPersistent = loginModel.RememberMe
                                          }, identity);

                    return RedirectToAction("Index", "Account");
                }
            }

            return View("Login", loginModel);
        }


        [Route("logout")]
        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }
    }
}