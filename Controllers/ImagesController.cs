using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            try
            {
                var images = await _imageRepository.GetAllBlogImages();

                //Convert Domain model to DTO
                var response = new List<BlogImageDTO>();

                foreach (var image in images)
                {
                    response.Add(new BlogImageDTO
                    {
                        Id = image.Id,
                        Title = image.Title,
                        CreatedDate = image.CreatedDate,
                        FileExtension = image.FileExtension,
                        FileName = image.FileName,
                        Url = image.Url,
                    });
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] IFormFile objfile, [FromForm] string fileName, [FromForm] string title)
        {
            if (ModelState.IsValid)
            {
                //
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(objfile.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    CreatedDate = DateTime.Now,
                };

                blogImage = await _imageRepository.Upload(objfile, blogImage);

                //Convert domainmodel to DTO

                var Response = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    CreatedDate = blogImage.CreatedDate,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                };

                return Ok(Response);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload([FromForm] IFormFile objfile)
        {
            var allowerExt = new string[] { ".jpg", ".jpeg", ".png" };


            if(allowerExt.Contains(Path.GetExtension(objfile.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "UnSupported File Format");
            }

            if (objfile.Length > 10485760)
            {
                ModelState.AddModelError("file", "UnSupported File Format");
            }
        }
    }
}
