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
        }
    }
}
