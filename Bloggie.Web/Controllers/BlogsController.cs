using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly IUserRepository userRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository,
            IUserRepository userRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.userRepository = userRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();

            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {

                    // Try to find the user ID in the JWT claims (usually stored in "NameIdentifier" claim)
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // This claim is typically used for the user ID in JWT

                    // Handle case if the user ID is missing
                    if (userIdClaim == null)
                    {
                        return Unauthorized("User ID not found in the token.");
                    }

                    // Proceed if user ID is available in the claim
                    string userId =userIdClaim.Value;


                    // Get like for this blog for this user
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                   

                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }


                }

                // Get comments for blog post
                var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    var userExist = await userRepository.GetByUserId(blogComment.UserId);
                    string Username = "Unknown";
                    if (userExist is not null)
                    {
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName;
                    }
                   
                    
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = Username
                    });
                }

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };

            }

            return View(blogDetailsViewModel);
        }



        [HttpGet]
        public async Task<IActionResult> GetMostLikedBlogs()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            // Create a list to hold posts with their like counts
            var blogPostWithLikes = new List<(BlogPost Post, int Likes)>();

            foreach (var post in blogPosts)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(post.Id);
                blogPostWithLikes.Add((post, totalLikes));
            }

            // Order by likes descending and take top 3
            var top3Posts = blogPostWithLikes
                .OrderByDescending(x => x.Likes)
                .Take(5)
                .Select(x => new
                {
                    title = x.Post.PageTitle,
                    shortDescription = x.Post.ShortDescription,
                    imageUrl = x.Post.FeaturedImageUrl,
                    urlHandle = x.Post.UrlHandle,
                    totalLikes = x.Likes,
                    author = x.Post.Author,
                    publishedDate = x.Post.PublishedDate
                });

            return Json(top3Posts);
        }


        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
         {
            // Check if the user is authenticated
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Try to find the user ID in the JWT claims (usually stored in "NameIdentifier" claim)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // This claim is typically used for the user ID in JWT

                // Handle case if the user ID is missing
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID not found in the token.");
                }

                // Proceed if user ID is available in the claim
                var userId = Guid.Parse(userIdClaim.Value);

                // Create the blog comment object
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = userId,  // Use the extracted user ID from the JWT token
                    DateAdded = DateTime.Now
                };

                // Add the comment to the repository
                await blogPostCommentRepository.AddAsync(domainModel);

                // Redirect to the Blogs page with the URL handle as a parameter
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            // If the user is not authenticated, return the login page or show an error
            return Unauthorized("You need to be logged in to post a comment.");
        }

    }
}
