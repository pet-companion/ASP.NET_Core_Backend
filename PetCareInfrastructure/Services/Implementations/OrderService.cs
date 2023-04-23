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
            return new APIResponse<List<OrderVM>>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Orders List", ordersVM.Count(), ordersVM);
        }

        public async Task<APIResponse<OrderVM>> GetOrder(int orderId)
        {
            var order = await _dbContext.Orders
                .FindAsync(orderId);
            if (order is null)
            {
                return new APIResponse<OrderVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Order Not Found", null);
            }
            var orderVM = _Mapper.Map<OrderVM>(order);
            return new APIResponse<OrderVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Store Data", 1, orderVM);
        }

        public async Task<APIResponse> AddOrder(AddOrderDto orderData)
        {
            var discountQty = orderData.Qty;
            if (orderData is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            if (orderData.Qty < 1)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Enter a Quantity Greater Than 0");
            }
            //check if CategoryId and StoreId found
            var product = _dbContext.Products.Find(orderData.ProductId);
            var user = _dbContext.Users.Find(orderData.UserId);
            if (product is null || user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Or Product Not Found");
            }
            //check if found enough Quantity or not in product
            if (product.Qty < discountQty)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, $"There is not enough product quantity, the remaining quantity is: {product.Qty}");
            }
            product.Qty = product.Qty - discountQty;
            var order = _Mapper.Map<Order>(orderData);
            order.PriceOnDemand = product.Price;
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Order Added Successfully");
        }

        public async Task<APIResponse> UpdateOrder(UpdateOrderDto orderData)
        {
            var discountQty = orderData.Qty;
            if (orderData is null || orderData.Id is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            if (orderData.Qty < 1)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Enter a Quantity Greater Than 0");
            }
            //check if CategoryId and StoreId found
            var product = _dbContext.Products.Find(orderData.ProductId);
            var user = _dbContext.Users.Find(orderData.UserId);
            if (product is null || user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Or Product Not Found");
            }
            //check if found enough Quantity or not in product
            if (product.Qty < discountQty)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, $"There is not enough product quantity, the remaining quantity is: {product.Qty}");
            }
            var dbOrder = await _dbContext.Orders.FindAsync(orderData.Id);
            if (dbOrder is null)
            {
                return new APIResponse<StoreVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Order Not Found", null);
            }
            //prevent update if status approved or rejected
            if (dbOrder.Status != OrderStatusEnum.Pending)
            {
                return new APIResponse<StoreVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, $"you can't update order data because the status is: {dbOrder.Status.ToDisplayName()}", null);
            }
            //Calc Product Qty
            discountQty = orderData.Qty - dbOrder.Qty;

            //check if found enough Quantity or not in product
            if (product.Qty < discountQty)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, $"There is not enough product quantity, the remaining quantity is: {product.Qty}");
            }
            //update product data
            product.Qty = product.Qty - discountQty;
            product.LastUpdatedAt = DateTime.Now;
            //update order data
            _Mapper.Map(orderData, dbOrder);
            dbOrder.PriceOnDemand = product.Price;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Order Updated Successfully");
        }

        public async Task<APIResponse> DeleteOrder(int orderId)
        {
            var order = await _dbContext.Orders
                .FindAsync(orderId);
            if (order is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Order Not Found");
            }
            order.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Order Deleted Successfully");
        }
    }
}
