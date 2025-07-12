using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<(bool isSuccess, string errMsg)> CreateCategory(Category category);
        Task<IEnumerable<Category>> GetAllCategories(string? query = null, string? sortBy = null, string? sortDirection = null, int? pageNumber = 1,  int? pageSize = 10);
        Task<Category?> GetCategoryById(Guid id);
        Task<Category?> UpdateCategory(Category objcategory);
        Task<Category?> DeleteCategory(Guid id);
        Task<int> GetCount();

    }
}
