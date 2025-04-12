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
            // Get counts for Blog Posts, Tags, Users, and Admins
            var blogCount = await blogPostRepository.GetCountAsync();
            var tagCount = await tagRepository.GetCountAsync();
            var userCount = await userRepository.GetCountAsync();
            var adminCount = await userRepository.GetAdminCountAsync();

            // Get analytics for Visits (Today, Weekly, Monthly)
            var todayVisits = await analyticsRepository.GetTodayVisitsAsync();
            var weeklyVisits = await analyticsRepository.GetWeeklyVisitsAsync();
            var monthlyVisits = await analyticsRepository.GetMonthlyVisitsAsync();

            // Get counts for Blog Posts created Today, This Week, This Month, This Year
            var today = DateTime.UtcNow.Date;
            var weekStart = today.AddDays(-(int)today.DayOfWeek); // Start of the week
            var monthStart = new DateTime(today.Year, today.Month, 1); // Start of the month
            var yearStart = new DateTime(today.Year, 1, 1); // Start of the year
            var lastYearStart = new DateTime(today.Year - 1, 1, 1); // Start of last year
           
            var todayBlogCount = await blogPostRepository.CountByConditionAsync(x => x.PublishedDate.Date == today);
            var weeklyBlogCount = await blogPostRepository.CountByConditionAsync(x => x.PublishedDate >= weekStart);
            var monthlyBlogCount = await blogPostRepository.CountByConditionAsync(x => x.PublishedDate >= monthStart);
            var yearlyBlogCount = await blogPostRepository.CountByConditionAsync(x => x.PublishedDate >= yearStart);

            // Prepare the model to pass to the view
            var model = new DashboardViewModel
            {
                BlogCount = blogCount,
                TagCount = tagCount,
                UserCount = userCount,
                AdminCount = adminCount,
                TodayVisits = todayVisits,
                WeeklyVisits = weeklyVisits,
                MonthlyVisits = monthlyVisits,
                PostsToday = todayBlogCount,
                PostsThisWeek = weeklyBlogCount,
                PostsThisMonth = monthlyBlogCount,
                PostsThisYear = yearlyBlogCount
            };

            return View(model);
        }






    }
}



