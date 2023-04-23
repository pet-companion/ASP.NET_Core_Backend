using AutoMapper;
using PetCareCore.Dto;
using PetCareCore.Enum;
using PetCareCore.ViewModel;
using PetCareData.Models;
using System;

namespace PetCareInfrastructure.AutoMapper
{
    public class MapperProfileCollection : Profile
    {
        public MapperProfileCollection()
        {
            //Authentication
            CreateMap<User, UserDataVM>();
            CreateMap<AddNewUserDto, User>();
            //Breed
            CreateMap<Breed, BreedVM>();
            //Pet
            CreateMap<Pet, PetVM>();
            CreateMap<AddPetDto, Pet>();
            CreateMap<UpdatePetDto, Pet>()
                .ForMember(vm => vm.LastUpdatedAt, m => m.MapFrom(x => DateTime.Now));
            //Category
            CreateMap<Category, CategoryVM>();
            //Product
            CreateMap<Product, ProductVM>();
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForMember(vm => vm.LastUpdatedAt, m => m.MapFrom(x => DateTime.Now));
            //Store
            CreateMap<Store, StoreVM>();
            CreateMap<AddStoreDto, Store>();
            CreateMap<UpdateStoreDto, Store>()
                .ForMember(vm => vm.LastUpdatedAt, m => m.MapFrom(x => DateTime.Now));
            //Order
            CreateMap<Order, OrderVM>()
                .ForMember(vm => vm.Status, m => m.MapFrom(x => x.Status.ToDisplayName()))
                .ForMember(vm => vm.TotalAmount, m => m.MapFrom(x => x.Qty * x.PriceOnDemand));
            CreateMap<AddOrderDto, Order>()
                .ForMember(vm => vm.Status, m => m.MapFrom(x => OrderStatusEnum.Pending));
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(vm => vm.LastUpdatedAt, m => m.MapFrom(x => DateTime.Now));
        }
    }
}
