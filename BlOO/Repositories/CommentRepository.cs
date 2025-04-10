using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        BlooContext blooContext;
        public CommentRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(Comment entity)
        {
            blooContext.comments.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Comment comment = GetById(id);
            blooContext.comments.Remove(comment);
        }

        public List<Comment> GetAll()
        {
            return blooContext.comments.ToList();
        }

        public Comment GetById(int id)
        {
            return blooContext.comments.FirstOrDefault(c => c.Id == id);
        }

        public void Insert(Comment entity)
        {
            blooContext.comments.Add(entity);
            NewCommentNotification(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Comment entity)
        {
            blooContext.comments.Update(entity);
        }

        public List<Comment> GetCommmentsByPostId(int postId)
        {
            return blooContext.comments.Include(c => c.User).Where(c => c.PostId == postId).ToList();

        }

        public void DeleteByPostId(int postId)
        {
            var comments = blooContext.comments.Where(c => c.PostId == postId).ToList();

            if (comments.Any())
            {
                blooContext.comments.RemoveRange(comments);
            }
        }
        //================================ Helper Methods ========================\\
        private void NewCommentNotification(Comment comment)
        {
            Notification notification = new Notification();
            var post = blooContext.posts.FirstOrDefault(p => p.Id == comment.PostId);
            var postOwner = blooContext.applicationUsers.FirstOrDefault(u => u.Id == post.UserId);
            var user = blooContext.applicationUsers.FirstOrDefault(u => u.Id == comment.UserId);
            notification.UserId = postOwner.Id;
            notification.ActorId = comment.UserId;
            notification.NotificationMessage = $"New comment from {user.FirstName} {user.LastName} on your post.";
            notification.ReferenceId = comment.Id;
            notification.NotificationType = Models.NotificationType.Comment;
            blooContext.notifications.Add(notification);
            blooContext.SaveChanges();
        }

    }
}
