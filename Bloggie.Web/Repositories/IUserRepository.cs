using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll(
             string? searchQuery = null,
             string? sortBy = null,
             string? sortDirection = null,
             int pageNumber = 1,
             int pageSize = 100);


        Task<int> CountAsync();

        Task<int> SearchCountAsync(string searchQuery);

        Task<int> GetCountAsync();

        Task<int> GetAdminCountAsync();
    }
}
