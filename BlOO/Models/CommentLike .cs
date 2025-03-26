using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{
    [Index(nameof(UserId),nameof(CommentId),IsUnique =true)]
    public class CommentLike
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public DateTime LikeDate { get; set; }
        //public virtual User? User { get; set; }
        //public virtual Comment? Comment { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Comment_Like>().HasIndex(c => new { c.user_id, c.comment_id }).IsUnique();
        //    modelBuilder.Entity<Comment_Like>().Property(c => c.like_date).HasDefaultValueSql("GETDATE()");
    }
}
