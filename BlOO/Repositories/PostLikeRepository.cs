using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        BlooContext blooContext;
        public PostLikeRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(PostLike entity)
        {
            blooContext.postLikes.Remove(entity);
        }

        public void DeleteById(int id)
        {
            PostLike postLike=GetById(id);
            blooContext.postLikes.Remove(postLike);
        }

        public List<PostLike> GetAll()
        {
            return blooContext.postLikes.ToList();
        }

        public PostLike GetById(int id)
        {
            return blooContext.postLikes.FirstOrDefault(l => l.Id == id);
        }

        public void Insert(PostLike entity)
        {
            blooContext.postLikes.Add(entity);
        }

        public void Save()
        {
          blooContext.SaveChanges();
        }

        public void Update(PostLike entity)
        {
            blooContext.postLikes.Update(entity);
        }

        public List<PostLike> GetAllPostLikesByPostId(int postId)
        {
            return blooContext.postLikes.Include(l => l.User).Where(l => l.PostId == postId).ToList();
        }

        public void DeleteLikesByPostId(int postId)
        {
            var likes = blooContext.postLikes.Where(l => l.PostId == postId).ToList();
            if (likes.Any())
            {
                blooContext.postLikes.RemoveRange(likes);
            }
        }

    }
}
