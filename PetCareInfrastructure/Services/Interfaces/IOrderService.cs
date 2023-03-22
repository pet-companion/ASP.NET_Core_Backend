using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        Task<APIResponse<List<OrderVM>>> GetOrderList();
        Task<APIResponse<OrderVM>> GetOrder(int orderId);
        Task<APIResponse> AddUpdateOrder(AddUpdateOrderDto orderData);
        Task<APIResponse> DeleteOrder(int orderId);
    }
}
