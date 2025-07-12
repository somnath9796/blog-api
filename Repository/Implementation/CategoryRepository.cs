using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this._dbContext = applicationDbContext;
        }

        public async Task<(bool isSuccess, string errMsg)> CreateCategory(Category category)
        {
            try
            {
                //Category name Should Be mandatory 
                if (category.Name == null || category.Name == "") 
                {
                    return (false, "Category Name Is mandatory");
                }

                //Check If Duplicate
                bool exists = await _dbContext.Categories.AnyAsync(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim());
                
                if(exists)
                {
                    return (false, "Category already exists");
                }


                await _dbContext.Categories.AddAsync(category); // Add new Category from DTO To Category 
                await _dbContext.SaveChangesAsync(); //Save Changes In DB

                return (true, "Category Added Successfully");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
            
        }

        public async Task<IEnumerable<Category>> GetAllCategories(string? query = null, string? sortBy = null, string? sortDirection = null,
            int? pageNumber = 1, int? pageSize = 10)
        {
            //Query 
            var categories = _dbContext.Categories.AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(query) == false)
            {
                categories = categories.Where(x=>x.Name.Contains(query));
            }
            //Sorting
            
            if(string.IsNullOrWhiteSpace(sortBy)==false)
            {
                //sort by name
                if (string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection,"asc",StringComparison.OrdinalIgnoreCase) ? true : false;
                    categories = isAsc ? categories.OrderBy(x=>x.Name) : categories.OrderByDescending(x=>x.Name);
                }
                //sort by url
                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;
                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);
                }
            }

            //Pagination
            // Page Number 1 pageSize 5 - skip 0, take 5
            // Page Number 2 pageSize 5 - skip 5, take 5
            // Page Number 3 pageSize 5 - skip 10,take 5

            var skipResults = (pageNumber - 1) * pageSize;
            categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 100);


           return await categories.ToListAsync();

        }

       public async Task<Category?> GetCategoryById(Guid id)
        {
           return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateCategory(Category objcategory)
        {
            var findCat = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == objcategory.Id);

            if (findCat != null) { 
            
                _dbContext.Entry(findCat).CurrentValues.SetValues(objcategory);
                await _dbContext.SaveChangesAsync();
                return objcategory;
            }
            return null;
        }

        public async Task<Category?> DeleteCategory(Guid id)
        {
           var existingCategory =  await _dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingCategory == null) 
            {
                return null;
            }

            _dbContext.Categories.Remove(existingCategory);
            await _dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<int> GetCount()
        {
            return await _dbContext.Categories.CountAsync();
        }
    }
}
