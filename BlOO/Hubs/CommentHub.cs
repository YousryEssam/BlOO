using BlOO.Models;
using BlOO.Repositories;
using BlOO.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace BlOO.Hubs
{
    public class CommentHub : Hub
    {
        public ICommentRepository CommentRepository { get; }
        public IPostRepository PostRepository { get; }
        public IPostLikeRepository postLikeRepository { get; }
        public IApplicationUserRepository applicationUser { get; }

        public CommentHub(ICommentRepository commentRepository, IPostRepository postRepository, IPostLikeRepository postLikeRepository,IApplicationUserRepository applicationUser)
        {
            CommentRepository = commentRepository;
            PostRepository = postRepository;
            this.postLikeRepository = postLikeRepository;
            this.applicationUser = applicationUser;
        }



        public async Task writeComment(Comment commentData)
        {
            try
            {
                var comment = new Comment
                {
                    Content = commentData.Content,
                    CommentDate = DateTime.Now,
                    PostId = commentData.PostId,
                    UserId = commentData.UserId,
                    LikeCount = 0
                };
                Post post = PostRepository.GetById(comment.PostId);


                CommentRepository.Insert(comment);
                CommentRepository.Save();
               var user= applicationUser.GetById(comment.UserId);
                if (post != null)
                {
                    post.CommentCount++;
                    PostRepository.Save();


                    var commentViewModel = new 
                    {
                        Content = comment.Content,
                        CommentDate = comment.CommentDate,
                        LikeCount = comment.LikeCount,
                        PostId = comment.PostId,
                        UserId = comment.UserId,
                        userName = user.FirstName + " " + user.LastName,
                        image = user.ProfileImageUrl

                    };

                    await Clients.All.SendAsync("NewComment", commentViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error: " + ex.Message);
                throw;
            }
        }

        public async Task NewReact(PostLike postLike)
        {
            var existingLike = postLikeRepository.GetAll()
                .FirstOrDefault(p => p.UserId == postLike.UserId && p.PostId == postLike.PostId);
            var user = applicationUser.GetById(postLike.UserId);
            if (existingLike == null)
            {
                // Add like
                postLikeRepository.Insert(postLike);
                Post post = PostRepository.GetById(postLike.PostId);
                if (post != null)
                {
                    post.LikeCount++;
                    PostRepository.Save();
                }

                await Clients.All.SendAsync("NewReact", new
                {
                    PostId = postLike.PostId,
                    Change = 1, // 
                    exist=true,
                    userName=user.FirstName+" "+user.LastName,
                    image=user.ProfileImageUrl,
                    userId=user.Id
                });
            }
            else
            {
                // Remove like
                postLikeRepository.Delete(existingLike);
                Post post = PostRepository.GetById(postLike.PostId);
                if (post != null)
                {
                    post.LikeCount--;
                    PostRepository.Save();
                }

                await Clients.All.SendAsync("NewReact", new
                {
                    PostId = postLike.PostId,
                    Change = -1,
                    exist=false,
                    userName = user.FirstName + " " + user.LastName,
                    image = user.ProfileImageUrl,
                    userId = user.Id
                });
            }

        }

    }
}