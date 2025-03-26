using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Follow
    {
        public int Id { get; set; }


        [Required]
        public int FollowerId { get; set; }// uncompleted

        [Required]
        public int FollowingId { get; set; }// uncompleted

        public DateTime FollowingDate { get; set; } = DateTime.UtcNow;



    }
}
