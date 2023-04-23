using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareCore.Dto;
using PetCareCore.Enum;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetCareAPI.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IHttpContextAccessor httpContext, IOrderService orderService) : base(httpContext)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderList()
        {
            var res = await _orderService.GetOrderList();
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var res = await _orderService.GetOrder(orderId);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderDto orderData)
        {
            var res = await _orderService.AddOrder(orderData);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDto orderData)
        {
            var res = await _orderService.UpdateOrder(orderData);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var res = await _orderService.DeleteOrder(orderId);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
