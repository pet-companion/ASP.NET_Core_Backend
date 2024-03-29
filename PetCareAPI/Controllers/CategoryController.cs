﻿using Microsoft.AspNetCore.Http;
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
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(IHttpContextAccessor httpContext, ICategoryService categoryService) : base(httpContext)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryList()
        {
            var res = await _categoryService.GetCategoryList();
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var res = await _categoryService.GetCategory(categoryId);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto categoryData)
        {
            var res = await _categoryService.AddCategory(categoryData);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto categoryData)
        {
            var res = await _categoryService.UpdateCategory(categoryData);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var res = await _categoryService.DeleteCategory(categoryId);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
