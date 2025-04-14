namespace BlOO.ViewModels
{
    public class UpdateAccessViewModel
    {
        public int ProfileId { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Your Old Password")]
        public string OldPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Your New Password")]
        public string? NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm Your New Password")]
        [Compare("NewPassword", ErrorMessage = "Password confirmation does not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
