using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
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

        public DbSet<Books> Books { get; set; }

        public DbSet<CartEntity> Carts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(50);
                entity.Property(e => e.BookImage).HasMaxLength(300);
                entity.Property(e => e.BookName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(750);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<CartEntity>()
                                    .HasOne(c => c.Books)
                                    .WithMany(b => b.Carts)
                                    .HasForeignKey(c => c.BookId)
                                    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
