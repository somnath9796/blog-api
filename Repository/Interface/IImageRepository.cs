using CodePulse.API.Models.Domain;
using System.Net;

namespace CodePulse.API.Repository.Interface
{
    public interface IImageRepository
    {
        Task<IEnumerable<BlogImage>> GetAllBlogImages(); 
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
