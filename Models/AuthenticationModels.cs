using System.ComponentModel.DataAnnotations;

namespace MedienKultur.Gurps.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string PasswordRetype { get; set; }
    }


    public class ProfileModel
    {
        public string Email { get; set; }

        public string GravatarEmail { get; set; }        
    }
}