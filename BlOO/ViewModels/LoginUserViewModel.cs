using System.ComponentModel.DataAnnotations;

namespace BlOO.ViewModels
{
    public class LoginUserViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe  { get; set; }
    }
}
