using BlOO.Repositories;
using BlOO.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace BlOO.Hubs
{
    public class CommentHub : Hub
    {
       public CommentHub(ICommentRepository commentRepository)
        {
            CommentRepository = commentRepository;
        }

        public ICommentRepository CommentRepository { get; }

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
                    
                    CommentRepository.Insert(comment);
                    CommentRepository.Save();

                    
                    var commentViewModel = new CommentsViewModel
                    {
                        Content = comment.Content,
                        CommentDate = comment.CommentDate,
                        LikeCount = comment.LikeCount,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    };

                    await Clients.All.SendAsync("NewComment", commentViewModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Error: " + ex.Message);
                    throw;
                }
            }
        }

    }



