using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IProductService
    {
        Task<APIResponse<List<ProductVM>>> GetProductList();
        Task<APIResponse<ProductVM>> GetProduct(int productId);
        Task<APIResponse> AddProduct(AddProductDto productData);
        Task<APIResponse> UpdateProduct(UpdateProductDto productData);
        Task<APIResponse> DeleteProduct(int productId);
    }
}
