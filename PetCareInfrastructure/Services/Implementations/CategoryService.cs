using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetCareCore.Dto;
using PetCareCore.Enum;
using PetCareCore.ViewModel;
using PetCareData.Data;
using PetCareData.Models;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return new APIResponse<List<CategoryVM>>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Products Categories", categoryVM.Count(), categoryVM);
        }

        public async Task<APIResponse<CategoryVM>> GetCategory(int categoryId)
        {
            var category = await _dbContext.Categories
                .FindAsync(categoryId);
            if (category is null)
            {
                return new APIResponse<CategoryVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Category Not Found", null);
            }
            var categoryVM = _Mapper.Map<CategoryVM>(category);
            return new APIResponse<CategoryVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Category Data", 1, categoryVM);
        }

        public async Task<APIResponse> AddCategory(AddCategoryDto categoryData)
        {
            var category = new Category();
            if (categoryData is null || string.IsNullOrWhiteSpace(categoryData.Name))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Category Name Is Required");
            }
            category.Name = categoryData.Name;
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Category Added Successfully");
        }

        public async Task<APIResponse> UpdateCategory(UpdateCategoryDto categoryData)
        {
            if (categoryData is null || string.IsNullOrWhiteSpace(categoryData.Name))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Category Name Is Required");
            }
            var category = await _dbContext.Categories.FindAsync(categoryData.Id);
            if (category is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Category Not Found");
            }
            category.Name = categoryData.Name;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Category Updated Successfully");
        }

        public async Task<APIResponse> DeleteCategory(int categoryId)
        {
            var category = await _dbContext.Categories
                .FindAsync(categoryId);
            if (category is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Category Not Found");
            }
            category.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Category Deleted Successfully");
        }
    }
}
