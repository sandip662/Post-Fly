using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository
            )
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index(string? searchQuery,
         string? sortBy,
         string? sortDirection,
         int pageSize = 5,
         int pageNumber = 1)
        {


            var totalRecords = 0;

            if (searchQuery == null)
            {
                totalRecords = await blogPostRepository.CountAsync();

            }
            else
            {
                totalRecords = await blogPostRepository.SearchCountAsync(searchQuery);
            }

            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
            }


            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;


            // getting all blogs
            var blogPosts = await blogPostRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);
            var LatestPosts = await blogPostRepository.GetAllAsync(null, "PublishedDate", "Desc", 1, 7);
            // get all tags
            var tags = await tagRepository.GetAllAsync();

            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags,
                LatestBlogPosts = LatestPosts
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}