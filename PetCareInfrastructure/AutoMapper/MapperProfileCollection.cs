using AutoMapper;
using PetCareCore.Dto;
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
            CreateMap<Order, OrderVM>();
        }
    }
}
