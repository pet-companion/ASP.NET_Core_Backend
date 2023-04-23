using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<APIResponse<List<CategoryVM>>> GetCategoryList();
        Task<APIResponse<CategoryVM>> GetCategory(int categoryId);
        Task<APIResponse> AddCategory(AddCategoryDto categoryData);
        Task<APIResponse> UpdateCategory(UpdateCategoryDto categoryData);
        Task<APIResponse> DeleteCategory(int categoryId);
    }
}
