namespace BlOO.ViewModels
{
    public class EditProfileViewModel
    {

        public int ProfileId { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "First name cannot exceed 20 characters.")]
        public string FirstName { get; set; }
        
        
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Last name cannot exceed 20 characters.")]
        public string LastName { get; set; }
      
        [Display(Name = "Biography")]
        public string? Bio {  get; set; }


        [Display(Name = "Select Profile Image")]
        public string? ProfileImageUrl { get; set; } 


        [Display(Name = "Select Cover Image")]
        public string? CoverImageUrl { get; set; }


        [Display(Name = "Upload Profile Image")]
        public IFormFile? ProfileImage { get; set; }


        [Display(Name = "Upload Cover Image")]
        public IFormFile? CoverImage { get; set; }
    }
}
