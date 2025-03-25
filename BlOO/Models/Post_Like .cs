using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{

    [Index(nameof(User_id), nameof(Post_id), IsUnique = true)]
    public class Post_Like
    {

        int Like_id { get; set; } 
        [ForeignKey("User")]
        public int User_id { get; set; }
        [ForeignKey("Post")]
        public int Post_id { get; set; }
        public DateTime Like_date { get; set; }

       // public virtual User? User { get; set; }//not completed
        //public virtual Post? Post { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post_Like>().HasIndex(c => new { c.user_id, c.post_id }).IsUnique();
        //    modelBuilder.Entity<Post_Like>().Property(c => c.like_date).HasDefaultValueSql("GETDATE()");
    }
}
