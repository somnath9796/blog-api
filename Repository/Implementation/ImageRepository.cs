using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CodePulse.API.Repository.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbcontext;
        public ImageRepository(IWebHostEnvironment webHost,IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbcontext)
        {
            this._webHost = webHost;
            this._httpContextAccessor = httpContextAccessor;
            this._dbcontext = dbcontext;
        }

        public async Task<IEnumerable<BlogImage>> GetAllBlogImages()
        {
          return await _dbcontext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //var localPath = Path.Combine(_webHost.ContentRootPath,"Images",$"{blogImage.FileName}{blogImage.FileExtension}");
            var localPath = Path.Combine(_webHost.WebRootPath,"Images",$"{blogImage.FileName}{blogImage.FileExtension}");

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Update to DB

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            blogImage.Url = urlPath;

            await _dbcontext.BlogImages.AddAsync(blogImage);
            await _dbcontext.SaveChangesAsync();
            return blogImage;
        }
    }
}
