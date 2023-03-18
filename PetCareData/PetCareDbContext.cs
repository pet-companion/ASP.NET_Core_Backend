using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCareData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetCareData.Data
{
    public class PetCareDbContext : DbContext
    {
        public PetCareDbContext(DbContextOptions<PetCareDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.PermissionSeeder();
            builder.Entity<User>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Role>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Breed>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Pet>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Order>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Category>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Product>()
                .HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Store>()
                .HasQueryFilter(x => !x.IsDeleted);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }

    }
}
