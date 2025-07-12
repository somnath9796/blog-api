using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Implementation;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddCategory(CreateCategoryRequestDTO objcat)
        {
            try
            {
                //Map DTO to Domain Model
                var Category = new Category
                {
                    Name = objcat.Name,
                    UrlHandle = objcat.UrlHandle,
                };

                var result = await _categoryRepository.CreateCategory(Category);

                if (result.isSuccess == false)
                {
                    return Conflict(result.errMsg);
                }
                var response = new CategoryDTO { CategoryId = Category.Id, Name = Category.Name, UrlHandle = Category.UrlHandle };

                return Ok(response);

            }
            catch(Exception ex)  
            { 
            return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] string? query, 
            [FromQuery] string? sortBy, [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategories(query,sortBy,sortDirection,pageNumber,pageSize);

                var lstCategories = new List<CategoryDTO>();

                foreach (var category in categories) 
                { 
                    lstCategories.Add(new CategoryDTO 
                    { CategoryId = category.Id,
                        Name = category.Name, 
                        UrlHandle = category.UrlHandle 
                    });
                }

                return Ok(lstCategories);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            try
            {
                var count = await _categoryRepository.GetCount();

                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
            try
            {
            var exst = await _categoryRepository.GetCategoryById(id);

                if(exst == null)
                {
                    return NotFound("No category Found");
                }

                var response = new CategoryDTO
                {
                    CategoryId = exst.Id,
                    Name = exst.Name,
                    UrlHandle = exst.UrlHandle,
                };

                return Ok(response);
            }
            catch (Exception ex) {
            return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDTO objUpdate)
        {
            try
            {
                //Convert To domain Model

                var objCategory = new Category
                {
                    Id = id,
                    Name = objUpdate.Name,
                    UrlHandle = objUpdate.UrlHandle,
                };

                objCategory = await _categoryRepository.UpdateCategory(objCategory);

                if(objCategory == null)
                {
                    return NotFound();
                }

                //Convert Domain To DTO

                var resultDTO = new CategoryDTO
                {
                    CategoryId = objCategory.Id,
                    Name = objCategory.Name,
                    UrlHandle = objCategory.UrlHandle,
                };
                return Ok(resultDTO);

            }

            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            { 
            var category = await _categoryRepository.DeleteCategory(id);
                if(category == null)
                {
                    return NotFound();
                }

                var resultDTO = new CategoryDTO
                {
                    CategoryId = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                };
                return Ok(resultDTO);

            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
