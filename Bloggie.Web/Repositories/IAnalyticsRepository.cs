namespace Bloggie.Web.Repositories
{
    public interface IAnalyticsRepository
    {


        Task<int> GetTodayVisitsAsync();
        Task<int> GetWeeklyVisitsAsync();
        Task<int> GetMonthlyVisitsAsync();


    }
}
