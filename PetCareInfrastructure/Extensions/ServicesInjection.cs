using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCareData.Data;
using PetCareInfrastructure.AutoMapper;
using PetCareInfrastructure.Services.Implementations;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugStoreInfrastructure.Extensions
{
    public static class ServicesInjection
    {
        public static void AddPetCareDependancy(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<PetCareDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(MapperProfileCollection).Assembly);

            //Dependancy Injection
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IBreedService, BreedService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
