using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetCareCore.Constant;
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
    public class ProductService : IProductService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;
        private readonly IFileService _fileService;

        public ProductService(PetCareDbContext dbContext, IMapper mapper, IFileService fileService)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
            _fileService = fileService;
        }

        public async Task<APIResponse<List<ProductVM>>> GetProductList()
        {
            var productList = await _dbContext.Products.ToListAsync();
            var productVM = _Mapper.Map<List<ProductVM>>(productList);
            //foreach (var product in productVM)
            //{
            //    product.ProductImg = await _fileService.GetFile(product.ImgName, FileFolder.FolderName);
            //}
            return new APIResponse<List<ProductVM>>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Products List", productVM.Count(), productVM);
        }

        public async Task<APIResponse<ProductVM>> GetProduct(int productId)
        {
            var product = await _dbContext.Products
                .FindAsync(productId);
            if (product is null)
            {
                return new APIResponse<ProductVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Product Not Found", null);
            }
            var productVM = _Mapper.Map<ProductVM>(product);
            //productVM.ProductImg = await _fileService.GetFile(product.ImgName, FileFolder.FolderName);
            return new APIResponse<ProductVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Product Data", 1, productVM);
        }

        public async Task<APIResponse> AddProduct(AddProductDto productData)
        {
            if (productData is null || string.IsNullOrWhiteSpace(productData.Name) || string.IsNullOrWhiteSpace(productData.Description))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            if (productData.Price < 1)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Enter a Price Greater Than 0");
            }
            if (productData.Qty < 1)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Enter a Quantity Greater Than 0");
            }
            //check if CategoryId, userId and StoreId is found
            var category = _dbContext.Categories.Find(productData.CategoryId);
            var store = _dbContext.Stores.Find(productData.StoreId);
            var user = _dbContext.Users.Find(productData.UserId);
            if (category is null || store is null || user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User, Category Or Store Not Found");
            }
            var product = _Mapper.Map<Product>(productData);
            if (productData.ProductImg?.Length > 0)
            {
                product.ImgName = await _fileService.SaveFile(productData.ProductImg, FileFolder.FolderName);
            }
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Product Added Successfully");
        }

        public async Task<APIResponse> UpdateProduct(UpdateProductDto productData)
        {
            if (productData is null || productData.Id is null || string.IsNullOrWhiteSpace(productData.Name) || string.IsNullOrWhiteSpace(productData.Description))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            if (productData.Price < 1)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Enter a Price Greater Than 0");
            }
            if (productData.Qty < 1)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Enter a Quantity Greater Than 0");
            }
            //check if CategoryId, userId and StoreId is found
            var category = _dbContext.Categories.Find(productData.CategoryId);
            var store = _dbContext.Stores.Find(productData.StoreId);
            var user = _dbContext.Users.Find(productData.UserId);
            if (category is null || store is null || user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User, Category Or Store Not Found");
            }
            var product = await _dbContext.Products.FindAsync(productData.Id);
            if (product is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Product Not Found");
            }
            _Mapper.Map(productData, product);
            if (productData.ProductImg?.Length > 0)
            {
                product.ImgName = await _fileService.SaveFile(productData.ProductImg, FileFolder.FolderName);
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Product Updated Successfully");
        }

        public async Task<APIResponse> DeleteProduct(int productId)
        {
            var product = await _dbContext.Pets
                .FindAsync(productId);
            if (product is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Product Not Found");
            }
            product.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Product Deleted Successfully");
        }
    }
}
