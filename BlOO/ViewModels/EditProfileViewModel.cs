namespace BlOO.ViewModels
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "First name cannot exceed 20 characters.")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Last name cannot exceed 20 characters.")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Bio {  get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public string? ProfileImageUrl { get; set; } 
        public string? CoverImageUrl { get; set; } 
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
