using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Models
{
    public class BlooContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
            public BlooContext() { }

            public BlooContext(DbContextOptions<BlooContext> options) : base(options)
            {

            }

            public DbSet<ApplicationUser> applicationUsers { get; set; }
            public DbSet<Comment> comments { get; set; }
            public DbSet<CommentLike> commentLikes { get; set; } 
            public DbSet<Follow> follows { get; set; }
            public DbSet<Message> messages { get; set; }
            public DbSet<Notification> notifications { get; set; }  
            public DbSet<Post> posts { get; set; }
            public DbSet<PostLike> postLikes { get; set; }
            public DbSet<PostReport> postReports { get; set; }
            public DbSet<Repost> reposts { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Notification>()
                    .HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Notification>()
                    .HasOne(n => n.Actor)
                    .WithMany(a => a.NotificationsSent)
                    .HasForeignKey(n => n.ActorId)
                    .OnDelete(DeleteBehavior.NoAction); 
            }

    }
}
