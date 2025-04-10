using BlOO.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPostReportRepository postReportRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IPostLikeRepository postLikeRepository;



        public AdminController(IPostReportRepository postReportRepository,
            IApplicationUserRepository applicationUserRepository , IPostRepository postRepository,
            ICommentRepository commentRepository , IPostLikeRepository postLikeRepositor)
        {
            this.postReportRepository = postReportRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.postRepository = postRepository;   
            this.commentRepository = commentRepository;
            this.postLikeRepository = postLikeRepositor;


        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            List<PostReport> reports = postReportRepository.GetAll();
            List<ReportPostWithReporterNameVM> result = new List<ReportPostWithReporterNameVM>();

            foreach (var report in reports)
            {
                ApplicationUser user = applicationUserRepository.GetById(report.ReporterId); 

                result.Add(new ReportPostWithReporterNameVM
                {
                    UserId = report.ReporterId,
                    PostId = report.PostId,
                    Name = $"{user.FirstName} {user.LastName}",
                    ImgUrl = user.ProfileImageUrl,
                    Content = report.Reason,
                    Id = report.Id
                });
            }

            return View("Admin", result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ShowPost(int PostId)
        {
            Post post = postRepository.GetById(PostId);
           ApplicationUser user =  applicationUserRepository.GetById(post.UserId);
            PostViewModel model = new PostViewModel()
            {
                Content=   post.Content,
                Id = post.Id,
                OwnerId = post.UserId,
                UserName = $"{user.FirstName} {user.FirstName}",
                UserImgUrl = user.ProfileImageUrl,
                ImgUrl = post.ImgUrl,
                LikeCount = post.LikeCount,
                RepostCount= post.RepostCount,
                CommentCount = post.CommentCount

            };
            return View("PostAdmin", model);
        }

        [Authorize(Roles = "Admin")]
        //no action
        public IActionResult DeletePostFromreportposts(int ReportId)
        {
            postReportRepository.DeleteById(ReportId);
            postReportRepository.Save();

            return RedirectToAction("AdminPage");


        }

        [Authorize(Roles = "Admin")]
        //delete post
        public IActionResult DeletePost(int postId, int ReptId)
        {
            var post = postRepository.GetByIdWithComments(postId);
            if (post == null)
            {
                return NotFound();
            }
            // احذف كل لايك المرتبطة بالبوست
            postLikeRepository.DeleteLikesByPostId(postId);
            // احذف كل التعليقات المرتبطة بالبوست
            commentRepository.DeleteByPostId(postId);
            // 3. حذف التقارير
            postReportRepository.DeleteReportsByPostId(postId);

            postRepository.DeleteById(postId);

            postRepository.Save(); 

            return RedirectToAction("AdminPage", new { ReportId = ReptId });
        }





    }
}
