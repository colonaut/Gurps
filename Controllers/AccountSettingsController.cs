using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.RavenDB;
using MedienKultur.Gurps.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    [Authorize]
    public class AccountSettingsController : Controller
    {
        readonly IDocumentSession _ravenSession;

        public AccountSettingsController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }


        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private UserManager<ApplicationUser> ApplicationUserManager
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


        [Route("password")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [Route("profile")]
        [HttpGet]
        public ActionResult Index()
        {
            var applicationUser = ApplicationUserManager.FindById(User.Identity.GetUserId());
            var profileModel = new ProfileModel
                               {
                                   Email = applicationUser.Email,
                                   GravatarEmail = applicationUser.GravatarEmail
                               };

            return View(profileModel);
        }

        [Route("profile")]
        [HttpPost]
        public ActionResult Index(ProfileModel profileModel)
        {
            var applicationUser = ApplicationUserManager.FindById(User.Identity.GetUserId());

            if (!string.IsNullOrWhiteSpace(profileModel.GravatarEmail))
            {
                applicationUser.Email = profileModel.Email;
                applicationUser.GravatarEmail = profileModel.GravatarEmail;
                applicationUser.GravatarUrl = GetGravatarUrl(profileModel.GravatarEmail);

                _ravenSession.Store(applicationUser, applicationUser.Id);                
            }

            return View(profileModel);
        }


        const string GravatarBaseUrl = "https://www.gravatar.com/avatar/";

        static string GetGravatarUrl(string email)
        {
            var encoder = new UTF8Encoding();
            var md5 = new MD5CryptoServiceProvider();
            var hashedBytes = md5.ComputeHash(encoder
                .GetBytes(email
                    .Trim()
                    .ToLowerInvariant()));
            var sb = new StringBuilder(hashedBytes.Length * 2);

            for (var i = 0; i < hashedBytes.Length; i++)
                sb.Append(hashedBytes[i].ToString("X2").ToLowerInvariant());

            return GravatarBaseUrl + sb.ToString();
        }

    }
}
