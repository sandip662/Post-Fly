using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int BlogCount { get; set; }
        public int TagCount { get; set; }
        public int UserCount { get; set; }
        public int AdminCount { get; set; }
        public int TodayVisits { get; set; }
        public int WeeklyVisits { get; set; }
        public int MonthlyVisits { get; set; }
        public int PostsToday { get; set; }
        public int PostsThisWeek { get; set; }
        public int PostsThisMonth { get; set; }
        public int PostsThisYear { get; set; }

        public Dictionary<int, int> YearWisePostCounts { get; set; }
    }





}
