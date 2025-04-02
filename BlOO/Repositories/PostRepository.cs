using BlOO.Models;
using Microsoft.Extensions.Hosting;

namespace BlOO.Repositories
{
    public class PostRepository : IPostRepository
    {
        BlooContext blooContext;
        public PostRepository(BlooContext blooContext) { 
            this.blooContext = blooContext;   
        }
        public void Delete(Post entity)
        {
            blooContext.posts.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Post post = GetById(id);
            blooContext.posts.Remove(post);
        }

        public List<Post> GetAll()
        {
            return blooContext.posts.ToList();
        }

        public Post GetById(int id)
        {
            return blooContext.posts.FirstOrDefault(p => p.Id == id);
        }

        public void Insert(Post entity)
        {
            blooContext.posts.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Post entity)
        {
            blooContext.posts.Update(entity);
        }
    }
}
