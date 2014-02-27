using AspNet.Identity.RavenDB;

namespace MedienKultur.Gurps.Models
{

    public class ApplicationUser : IdentityUser
    {
        private ApplicationUser(){}

        public ApplicationUser(string email)
            : base(email)
        {
            //base.Id = Guid.NewGuid().ToString();     
            base.UserName = email;
            Email = email;
        }

        public ApplicationUser(string email, string userName)
            : base(userName)
        {
            Email = email;
        }


        public string Email { get; set; }

        public string GravatarEmail { get; set; }

        public string GravatarUrl { get; set; }
        
    }

}