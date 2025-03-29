using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }


        public async Task<IEnumerable<IdentityUser>> GetAll(string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber = 1,
            int pageSize = 100)
        {

            var query = authDbContext.Users.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.UserName.Contains(searchQuery) ||
                                         x.Email.Contains(searchQuery));
            }


            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "UserName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);
                }

                if (string.Equals(sortBy, "Email", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email);
                }
            }



            // Pagination
            // Skip 0 Take 5 -> Page 1 of 5 results
            // Skip 5 Take next 5 -> Page 2 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

             var users = await query.ToListAsync();
        

            var superAdminUser = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }
           
            return users;
        }




        public async Task<int> CountAsync()
        {
            return await authDbContext.Users.CountAsync();
        }

        public async Task<int> SearchCountAsync(string searchQuery)
        {
            return await authDbContext.Users
                .Where(x => x.UserName.Contains(searchQuery))
                .CountAsync();
        }
    }
}
