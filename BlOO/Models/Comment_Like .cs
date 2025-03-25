using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{
    [Index(nameof(User_id),nameof(Comment_id),IsUnique =true)]
    public class Comment_Like
    {

        public int Like_id { get; set; }
        [ForeignKey("User")]
        public int User_id { get; set; }
        [ForeignKey("Comment")]
        public int Comment_id { get; set; }
        public DateTime Like_date { get; set; }
        //public virtual User? User { get; set; }
        //public virtual Comment? Comment { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Comment_Like>().HasIndex(c => new { c.user_id, c.comment_id }).IsUnique();
        //    modelBuilder.Entity<Comment_Like>().Property(c => c.like_date).HasDefaultValueSql("GETDATE()");
    }
}
