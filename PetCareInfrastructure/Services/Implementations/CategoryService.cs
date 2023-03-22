using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetCareCore.Dto;
using PetCareCore.ViewModel;
using PetCareData.Data;
using PetCareData.Models;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;

        public CategoryService(PetCareDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
        }

        public async Task<APIResponse<List<CategoryVM>>> GetCategoryList()
        {
            var categoryList = await _dbContext.Categories.ToListAsync();
            var categoryVM = _Mapper.Map<List<CategoryVM>>(categoryList);
            return new APIResponse<List<CategoryVM>>(true, "All Categories", categoryVM, categoryVM.Count());
        }

        public async Task<APIResponse<CategoryVM>> GetCategory(int categoryId)
        {
            var category = await _dbContext.Categories
                .FindAsync(categoryId);
            if (category is null)
            {
                return new APIResponse<CategoryVM>(false, "Category Not Found", null, 0);
            }
            var categoryVM = _Mapper.Map<CategoryVM>(category);
            return new APIResponse<CategoryVM>(true, "Category Data", categoryVM, 1);
        }

        public async Task<APIResponse> AddUpdateCategory(AddUpdateCategoryDto categoryData)
        {
            var category = new Category();
            var msg = string.Empty;
            if (categoryData is null || string.IsNullOrWhiteSpace(categoryData.Name))
            {
                return new APIResponse(false, "Category Name Is Required");
            }
            //Edit
            if (categoryData.Id != null)
            {
                category = await _dbContext.Categories.FindAsync(categoryData.Id);
                if (category is null)
                {
                    return new APIResponse(false, "Category Not Found");
                }
                category.LastUpdatedAt = DateTime.Now;
                msg = "Category Updated Successfully";
            }
            category.Name = categoryData.Name;
            if (categoryData.Id == null)
            {
                await _dbContext.Categories.AddAsync(category);
                msg = "Category Added Successfully";
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, msg);
        }

        public async Task<APIResponse> DeleteCategory(int categoryId)
        {
            var category = await _dbContext.Categories
                .FindAsync(categoryId);
            if (category is null)
            {
                return new APIResponse(false, "Category Not Found");
            }
            category.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Category Deleted Successfully");
        }
    }
}
