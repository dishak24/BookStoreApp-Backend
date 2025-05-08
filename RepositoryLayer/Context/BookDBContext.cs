using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class BookDBContext : DbContext
    {
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options) { }


        //For create User table 
        public DbSet<UserEntity> Users { get; set; }

        //For create Admin table
        public DbSet<AdminEntity> Admins { get; set; }
    }
}
