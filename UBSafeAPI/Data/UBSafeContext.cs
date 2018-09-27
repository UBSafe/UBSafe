using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UBSafeAPI.Models;

namespace UBSafeAPI.Data
{
    public class UBSafeContext : DbContext
    {
        public UBSafeContext(DbContextOptions<UBSafeContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        //provides initialization of tables if they do not yet exist
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}