using System;
using System.Web.Http;
using System.Web.Mvc;
using AspNet.Identity.RavenDB;
using MedienKultur.Gurps;
using MedienKultur.Gurps.Controllers;
using MedienKultur.Gurps.DependencyResolution;
using MedienKultur.Gurps.Filters;
using MedienKultur.Gurps.Models;
using Microsoft.AspNet.Identity;
using Raven.Client;
using StructureMap;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(RavenDbUnitOfWorkFilterConfig), "Start")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(RavenDbUnitOfWorkStructureMapRegistry), "Start")]

namespace MedienKultur.Gurps
{
	public static class RavenDbUnitOfWorkFilterConfig
    {
        public static void Start()
        {
            //add raven attribute to filters for mvc (handles save and close).
            //Disable this method if you want to set the attribute manually for some controllers instead of all. Disable this method if you want to set the attribute manually for some controllers instead of all.
            GlobalFilters.Filters.Add(new RavenDbDocumentSessionAttribute());
            //add http raven attribute for WebApi to global filters (handles save and close of raven session for WebApi).
            //Disable this method if you want to set the attribute manually for some controllers instead of all.
            GlobalConfiguration.Configuration.Filters.Add(new RavenDbDocumentSessionHttpAttribute());
        }
    }
    
    public static class RavenDbUnitOfWorkStructureMapRegistry
    {
        public static void Start()
        {
            //add additional StructureMapRavenDBRegistry to ObjectFactory's container _after_ (PostApplicationStart) the initialization is done (in StructuremapMvc).
            ObjectFactory.Configure(r => r.AddRegistry<StructureMapRavenDbRegistry>());
            
            //place your initial raven objects here for creation on app start, if you want to
            #region Initial RavenDB Content
            using (var session = ObjectFactory.GetInstance<IDocumentSession>())
            {
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(session));

                userManager.UserValidator =
                    new UserValidator<ApplicationUser>(userManager)
                    {
                        AllowOnlyAlphanumericUserNames = false
                    };

                var applicationUser =
                    new ApplicationUser("colonaut@gmx.de", "colonaut")
                    {
                        GravatarEmail = "colonaut@gmx.de"
                    };

                var identityResult =
                    userManager.Create(applicationUser,
                        "password");

                if (identityResult.Succeeded)
                {
                    userManager.AddToRole(applicationUser.Id,
                        "Administrator");

                    var claimsIdentity =
                        userManager.CreateIdentity(applicationUser,
                            DefaultAuthenticationTypes.ApplicationCookie);
                }
                session.SaveChanges();
            }
            #endregion
        }

    }

}