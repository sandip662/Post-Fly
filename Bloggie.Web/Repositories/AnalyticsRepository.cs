using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.EntityFrameworkCore;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly BloggieDbContext bloggieDbContext;

    public AnalyticsRepository(BloggieDbContext bloggieDbContext)
    {
        this.bloggieDbContext = bloggieDbContext;
    }

    public async Task<int> GetTodayVisitsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await bloggieDbContext.Visits
            .Where(v => v.VisitedAt.Date == today)
            .CountAsync();
    }

    public async Task<int> GetWeeklyVisitsAsync()
    {
        var weekStart = DateTime.UtcNow.Date.AddDays(-7);
        return await bloggieDbContext.Visits
            .Where(v => v.VisitedAt >= weekStart)
            .CountAsync();
    }

    public async Task<int> GetMonthlyVisitsAsync()
    {
        var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        return await bloggieDbContext.Visits
            .Where(v => v.VisitedAt >= monthStart)
            .CountAsync();



    }

    public class VisitTrackerMiddleware
    {
        private readonly RequestDelegate _next;

        public VisitTrackerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, BloggieDbContext dbContext)
        {
            var visit = new Visit
            {
                IPAddress = context.Connection.RemoteIpAddress?.ToString(),
                VisitedAt = DateTime.UtcNow
            };

            dbContext.Visits.Add(visit);
            await dbContext.SaveChangesAsync();

            await _next(context);
        }
    }



}
