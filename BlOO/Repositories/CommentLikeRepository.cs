using BlOO.Models;

namespace BlOO.Repositories
{
    public class CommentLikeRepository : ICommentLikeRepository
    {
        BlooContext blooContext;
        public CommentLikeRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(CommentLike entity)
        {
            blooContext.commentLikes.Remove(entity);
        }

        public void DeleteById(int id)
        {
            CommentLike commentLike = GetById(id);
            blooContext.commentLikes.Remove(commentLike);
        }

        public List<CommentLike> GetAll()
        {
            return blooContext.commentLikes.ToList();
        }

        public CommentLike GetById(int id)
        {
            return blooContext.commentLikes.FirstOrDefault(c => c.Id == id);
        }

        public void Insert(CommentLike entity)
        {
            blooContext.commentLikes.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(CommentLike entity)
        {
            blooContext.commentLikes.Update(entity);
        }
    }
}
