using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAnalyticsRepository analyticsRepository;

        public DashboardController (ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository, IAnalyticsRepository analyticsRepository,
        ITagRepository tagRepository, IUserRepository userRepository,
            UserManager<IdentityUser> userManager
            )
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.analyticsRepository = analyticsRepository;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogCount = await blogPostRepository.GetCountAsync();
            var tagCount = await tagRepository.GetCountAsync();
            var userCount = await userRepository.GetCountAsync();
            var adminCount = await userRepository.GetAdminCountAsync(); // Add this method
          
            var model = new DashboardViewModel
            {
                BlogCount = await blogPostRepository.GetCountAsync(),
                TagCount = await tagRepository.GetCountAsync(),
                UserCount = await userRepository.GetCountAsync(),
                AdminCount = await userRepository.GetAdminCountAsync(),
                TodayVisits = await analyticsRepository.GetTodayVisitsAsync(),
                WeeklyVisits = await analyticsRepository.GetWeeklyVisitsAsync(),
                MonthlyVisits = await analyticsRepository.GetMonthlyVisitsAsync()
            };
            ;

            return View(model);
        }





    }
}



