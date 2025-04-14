using BLOO.Hubs;
using BlOO.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlOO.Hubs;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace BlOO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<BlooContext>();

            builder.Services.AddDbContext<BlooContext>(Contextbuilder =>
            {

                Contextbuilder.UseSqlServer(builder.Configuration.GetConnectionString("YousryCS"));

                //Contextbuilder.UseSqlServer(builder.Configuration.GetConnectionString("Nourcs"));

                //Contextbuilder.UseSqlServer(builder.Configuration.GetConnectionString("MarlyCS"));

                //Contextbuilder.UseSqlServer(builder.Configuration.GetConnectionString("CS"));

            });

            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IFollowRepository, FollowRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            builder.Services.AddScoped<IPostReportRepository, PostReportRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IRepostRepository, RepostRepository>();
            builder.Services.AddSignalR();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 15 * 1024 * 1024; // 15MB limit
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 15 * 1024 * 1024; // 15MB limit
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.MapHub<CommentHub>("/PostComment");
            app.MapHub<ChatMessageHub>("/ChatMessage");
            app.MapHub<FollowersSystemHub>("/FollowersSystem");
            app.MapHub<NotificationsSystemHub>("/NotificationsSystem");
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Welcome}/{id?}");

            app.Run();
        }
    }
}
