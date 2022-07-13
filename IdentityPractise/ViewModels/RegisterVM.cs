using System.ComponentModel.DataAnnotations;

namespace IdentityPractise.ViewModels
{
    public class RegisterVM
    {
        [Required, StringLength(20)]
        public string Fullname { get; set; }
        [Required, StringLength(15)]
        public string Username { get; set; }



        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
