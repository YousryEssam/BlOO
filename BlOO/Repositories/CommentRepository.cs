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


    }
}
