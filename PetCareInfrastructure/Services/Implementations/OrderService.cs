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
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;

        public OrderService(PetCareDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
        }

        public async Task<APIResponse<List<OrderVM>>> GetOrderList()
        {
            var ordersList = await _dbContext.Orders.ToListAsync();
            var ordersVM = _Mapper.Map<List<OrderVM>>(ordersList);
            return new APIResponse<List<OrderVM>>(true, "All Orders List", ordersVM, ordersVM.Count());
        }

        public async Task<APIResponse<OrderVM>> GetOrder(int orderId)
        {
            var order = await _dbContext.Orders
                .FindAsync(orderId);
            if (order is null)
            {
                return new APIResponse<OrderVM>(false, "Order Not Found", null, 0);
            }
            var orderVM = _Mapper.Map<OrderVM>(order);
            return new APIResponse<OrderVM>(true, "Order Data", orderVM, 1);
        }

        public async Task<APIResponse> AddUpdateOrder(AddUpdateOrderDto orderData)
        {
            var order = new Order();
            var msg = string.Empty;
            var discountQty = orderData.Qty;
            if (orderData is null)
            {
                return new APIResponse(false, "All Fields Are Required");
            }
            if (orderData.Qty < 1)
            {
                return new APIResponse(false, "Please Enter a Quantity Greater Than 0");
            }
            //check if CategoryId and StoreId found
            var product = _dbContext.Products.Find(orderData.ProductId);
            var user = _dbContext.Users.Find(orderData.UserId);
            if (product is null || user is null)
            {
                return new APIResponse(false, "User Or Product Not Found");
            }
            //Edit
            if (orderData.Id != null)
            {
                order = await _dbContext.Orders.FindAsync(orderData.Id);
                if (order is null)
                {
                    return new APIResponse(false, "Order Not Found");
                }
                product.LastUpdatedAt = DateTime.Now;
                order.LastUpdatedAt = DateTime.Now;
                msg = "Order Updated Successfully";
                //Calc Product Qty
                discountQty = orderData.Qty - order.Qty;
            }
            //check if found enough Quantity or not in product
            if (product.Qty < discountQty)
            {
                return new APIResponse(true, $"There is not enough product, the remaining quantity is: {product.Qty}");
            }
            product.Qty = product.Qty - discountQty;

            order.Qty = orderData.Qty;
            order.PriceOnDemand = product.Price;
            order.Status = OrderStatusEnum.Pending;
            order.UserId = orderData.UserId;
            order.ProductId = orderData.ProductId;
            if (orderData.Id == null)
            {
                await _dbContext.Orders.AddAsync(order);
                msg = "Order Added Successfully";
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, msg);
        }

        public async Task<APIResponse> DeleteOrder(int orderId)
        {
            var order = await _dbContext.Orders
                .FindAsync(orderId);
            if (order is null)
            {
                return new APIResponse(false, "Order Not Found");
            }
            order.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Order Deleted Successfully");
        }
    }
}
