using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Implementation;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this._blogPostRepository = blogPostRepository;
            this._categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlogPosts([FromBody] CreateBlogPostsDTO objcreate)
        {
            try
            {
                var blogPost = new BlogPost
                {
                    Author = objcreate.Author,
                    Title = objcreate.Title,
                    FeaturedImageUrl = objcreate.FeaturedImageUrl,
                    IsVisible = objcreate.IsVisible,
                    PublishedDate = objcreate.PublishedDate,
                    ShortDescription = objcreate.ShortDescription,
                    UrlHandle = objcreate.UrlHandle,
                    Content = objcreate.Content,
                    Categories = new List<Category>(),
                };

                // Foreach Loop to add categories

                foreach (var categoryGuid in objcreate.Categories)
                {
                    var existingCategory = await _categoryRepository.GetCategoryById(categoryGuid);

                    if (existingCategory != null)
                    {
                        blogPost.Categories.Add(existingCategory);
                    }
                }

                blogPost = await _blogPostRepository.CreateBlogPost(blogPost);

                //Return data in DTO

                var Response = new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        CategoryId = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            try
            {
                var blogPosts = await _blogPostRepository.GetAllBlogPosts();

                var lstBlogPost = new List<BlogPostDTO>();

                foreach (var blogPost in blogPosts)
                {
                    lstBlogPost.Add(new BlogPostDTO
                    {
                        Id = blogPost.Id,
                        Author = blogPost.Author,
                        Content = blogPost.Content,
                        FeaturedImageUrl = blogPost.FeaturedImageUrl,
                        IsVisible = blogPost.IsVisible,
                        PublishedDate = blogPost.PublishedDate,
                        ShortDescription = blogPost.ShortDescription,
                        Title = blogPost.Title,
                        UrlHandle = blogPost.UrlHandle,
                        Categories = blogPost.Categories.Select(x => new CategoryDTO
                        {
                            CategoryId = x.Id,
                            Name = x.Name,
                            UrlHandle = x.UrlHandle
                        }).ToList()
                    });
                }

                return Ok(lstBlogPost);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetBlogPostsById([FromRoute] Guid Id)
        {
            try
            { 
            var blogPost = await _blogPostRepository.GetBlogPostById(Id);

                if (blogPost == null)
                {
                    return NotFound("No category Found");
                }

                var Response = new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        CategoryId = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

                return Ok(Response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrl([FromRoute] string urlHandle)
        {
            try
            {
                var blogPost = await _blogPostRepository.GetBlogPostByUrlHandle(urlHandle);

                if (blogPost == null)
                {
                    return NotFound("No Blog Post Found");
                }

                var Response = new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        CategoryId = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute]Guid Id, UpdateBlogPostDTO objblogPost)
        {
            try
            {
                //Convert DTO To Domain

                var blogPost = new BlogPost
                {
                    Id = Id,
                    Author = objblogPost.Author,
                    Title = objblogPost.Title,
                    FeaturedImageUrl = objblogPost.FeaturedImageUrl,
                    IsVisible = objblogPost.IsVisible,
                    PublishedDate = objblogPost.PublishedDate,
                    ShortDescription = objblogPost.ShortDescription,
                    UrlHandle = objblogPost.UrlHandle,
                    Content = objblogPost.Content,
                    Categories = new List<Category>(),
                };

                // Foreach Loop to add categories
                foreach (var categoryGuid in objblogPost.Categories)
                {
                    var existingCategory = await _categoryRepository.GetCategoryById(categoryGuid);

                    if (existingCategory != null)
                    {
                        blogPost.Categories.Add(existingCategory);
                    }
                }


                var updatedData = await _blogPostRepository.UpdateCategory(blogPost);


                if (updatedData == null) 
                {
                return NotFound("No Data Updated");
                }

                var Response = new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        CategoryId = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

                return Ok(Response);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid Id)
        {
            try
            {
                var existBlog = await _blogPostRepository.DeleteBlogPost(Id);

                if (existBlog == null)
                {
                    return NotFound("Blog Post Not found");
                }

                var Response = new BlogPostDTO
                {
                    Id = existBlog.Id,
                    Author = existBlog.Author,
                    Content = existBlog.Content,
                    FeaturedImageUrl = existBlog.FeaturedImageUrl,
                    IsVisible = existBlog.IsVisible,
                    PublishedDate = existBlog.PublishedDate,
                    ShortDescription = existBlog.ShortDescription,
                    Title = existBlog.Title,
                    UrlHandle = existBlog.UrlHandle,
                };

                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
