using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }


        public async Task<IEnumerable<Tag>> GetAllAsync(
    string? searchQuery = null,
    string? sortBy = null,
    string? sortDirection = null,
    int pageNumber = 1,
    int pageSize = 100)
        {
            var query = bloggieDbContext.Tags.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                         x.DisplayName.Contains(searchQuery));
            }

            // Sorting (Default to Name ascending)
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                else if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Name);
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }



        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }

        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();
        }
        public async Task<int> SearchCountAsync(string searchQuery)
        {
            return await bloggieDbContext.Tags
                .Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery))
                .CountAsync();
        }
    }
}
