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
            //Category
            CreateMap<Category, CategoryVM>();
            //Product
            CreateMap<Product, ProductVM>();
            //Store
            CreateMap<Store, StoreVM>();
            //Order
            CreateMap<Order, OrderVM>()
                .ForMember(vm => vm.Status, m => m.MapFrom(x => x.Status.ToDisplayName()))
                .ForMember(vm => vm.TotalAmount, m => m.MapFrom(x => x.Qty * x.PriceOnDemand));
        }
    }
}
