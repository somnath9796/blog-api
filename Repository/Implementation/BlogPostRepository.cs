using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BlogPostRepository(ApplicationDbContext applicationDbContext)
        {
            this._dbContext = applicationDbContext;
        }

        public async Task<BlogPost> CreateBlogPost(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPosts()
        {
                return await _dbContext.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }
        public async Task<BlogPost?> GetBlogPostById(Guid id)
        {
            return await _dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<BlogPost?> DeleteBlogPost(Guid id)
        {
            var existingblogposts = await _dbContext.BlogPosts.FirstOrDefaultAsync(x=>x.Id == id);


            if (existingblogposts != null) { 
            
                 _dbContext.BlogPosts.Remove(existingblogposts); 
                await _dbContext.SaveChangesAsync();
                return existingblogposts;
            }

            return null;
        }

        public async Task<BlogPost?> UpdateCategory(BlogPost objblogpost)
        {
            var existblogPost = await _dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id == objblogpost.Id);

            if (existblogPost == null)
            {
                return null;
            }

            //Update existing Blogpost

            _dbContext.Entry(existblogPost).CurrentValues.SetValues(objblogpost);

            //Update categories
            existblogPost.Categories = objblogpost.Categories;
            await _dbContext.SaveChangesAsync();
            return objblogpost;
        }

        public async Task<BlogPost?> GetBlogPostByUrlHandle(string UrlHandle)
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == UrlHandle);
        }
    }
}
