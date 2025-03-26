using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{

    [Index(nameof(UserId), nameof(PostId), IsUnique = true)]
    public class PostLike
    {

        int LikeId { get; set; } 
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public DateTime LikeDate { get; set; }

       // public virtual User? User { get; set; }//not completed
        //public virtual Post? Post { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post_Like>().HasIndex(c => new { c.user_id, c.post_id }).IsUnique();
        //    modelBuilder.Entity<Post_Like>().Property(c => c.like_date).HasDefaultValueSql("GETDATE()");
    }
}
