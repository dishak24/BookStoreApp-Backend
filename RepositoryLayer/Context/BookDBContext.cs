using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class BookDBContext : DbContext
    {
        public BookDBContext(DbContextOptions option) : base(option)  {  }

        //For create User table 
        public DbSet<UserEntity> Users { get; set; }

    }
}
