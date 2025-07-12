using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface IBlogPostRepository
    {
       Task<BlogPost> CreateBlogPost(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllBlogPosts();
        Task<BlogPost?> GetBlogPostById(Guid id);

        Task<BlogPost?> GetBlogPostByUrlHandle(string UrlHandle);
        Task<BlogPost?> UpdateCategory(BlogPost objblogpost);
        Task<BlogPost?> DeleteBlogPost(Guid id);
    }
}
