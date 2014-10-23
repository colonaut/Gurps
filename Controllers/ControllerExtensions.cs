using System.Web.Mvc;
using AspNet.Identity.RavenDB;
using MedienKultur.Gurps.Models;
using Microsoft.AspNet.Identity;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    public static class ControllerExtensions
    {
        public static UserManager<ApplicationUser> GetApplicationUserManager(this Controller controller, IDocumentSession ravenSession)
        {
            var userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ravenSession));
            userManager.UserValidator =
                new UserValidator<ApplicationUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };
            return userManager;
        }

        public static ApplicationUser GetApplicationUser(this Controller controller,
            IDocumentSession ravenSession)
        {
            return GetApplicationUserManager(controller, ravenSession)
                //.FindByName(controller.User.Identity.Name);
                .FindById(controller.User.Identity.GetUserId());
        }
    }
}