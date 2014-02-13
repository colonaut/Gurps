using System;
using MedienKultur.Identity.RavenDB;
using Microsoft.AspNet.Identity;

namespace MedienKultur.Gurps.Models
{

    public class ApplicationUser : IdentityUser
    {
        string _userName;
        string _email;

        public ApplicationUser(string email)
        {
            _email = email;
            Id = Guid.NewGuid().ToString();

        }

        public ApplicationUser(string email, string userName)
            : this(email)
        {
            _userName = userName;
        }


        public string Id { get; private set; }

        public string UserName
        {
            get { return _userName ?? _email; }
            set { _userName = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string GravatarEmail { get; set; } //private gravatar email
    }

}