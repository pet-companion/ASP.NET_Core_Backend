using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetCareCore.Constant;
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
            foreach (var product in productVM)
            {
                product.ProductImg = await _fileService.GetFile(product.ImgName, FileFolder.FolderName);
            }
            return new APIResponse<List<ProductVM>>(true, "All Product List", productVM, productVM.Count());
        }

        public async Task<APIResponse<ProductVM>> GetProduct(int productId)
        {
            var product = await _dbContext.Products
                .FindAsync(productId);
            if (product is null)
            {
                return new APIResponse<ProductVM>(false, "Product Not Found", null, 0);
            }
            var productVM = _Mapper.Map<ProductVM>(product);
            productVM.ProductImg = await _fileService.GetFile(product.ImgName, FileFolder.FolderName);
            return new APIResponse<ProductVM>(true, "Product Data", productVM, 1);
        }

        public async Task<APIResponse> AddUpdateProduct(AddUpdateProductDto productData)
        {
            var product = new Product();
            var msg = string.Empty;
            if (productData is null || string.IsNullOrWhiteSpace(productData.Name) || string.IsNullOrWhiteSpace(productData.Description))
            {
                return new APIResponse(false, "All Fields Are Required");
            }
            if (productData.Price < 1)
            {
                return new APIResponse(false, "Please Enter a Price Greater Than 0");
            }
            if (productData.Qty < 1)
            {
                return new APIResponse(false, "Please Enter a Quantity Greater Than 0");
            }
            //check if CategoryId and StoreId found
            var category = _dbContext.Categories.Find(productData.CategoryId);
            var store = _dbContext.Stores.Find(productData.StoreId);
            var user = _dbContext.Users.Find(productData.UserId);
            if (category is null || store is null || user is null)
            {
                return new APIResponse(false, "User, Category Or Store Not Found");
            }
            //Edit
            if (productData.Id != null)
            {
                product = await _dbContext.Products.FindAsync(productData.Id);
                if (product is null)
                {
                    return new APIResponse(false, "Product Not Found");
                }
                product.LastUpdatedAt = DateTime.Now;
                msg = "Product Updated Successfully";
            }
            product.Name = productData.Name;
            product.Description = productData.Description;
            product.Price = productData.Price;
            product.CategoryId = productData.CategoryId;
            product.StoreId = productData.StoreId;
            product.Qty = productData.Qty;
            product.UserId = productData.UserId;

            if (productData.ProductImg?.Length > 0)
            {
                product.ImgName = await _fileService.SaveFile(productData.ProductImg, FileFolder.FolderName);
            }

            if (productData.Id == null)
            {
                await _dbContext.Products.AddAsync(product);
                msg = "Product Added Successfully";
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, msg);
        }

        public async Task<APIResponse> DeleteProduct(int productId)
        {
            var product = await _dbContext.Pets
                .FindAsync(productId);
            if (product is null)
            {
                return new APIResponse(false, "Product Not Found");
            }
            product.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Product Deleted Successfully");
        }
    }
}
