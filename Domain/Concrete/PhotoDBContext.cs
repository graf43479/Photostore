using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Domain.Configuration;
using Domain.Entities;

namespace Domain.Concrete
{
    public class PhotoDBContext : DbContext
    {
        public PhotoDBContext() //: base("PhotoDBContext") 
        {
            //Database.SetInitializer<PhotoDBContext>(new DropCreateDatabaseAlways<PhotoDBContext>());
            
           // Database.SetInitializer(new PhotoDBInitializer());
            //поставить null на продакшене
             Database.SetInitializer<PhotoDBContext>(null);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ProductConfig());
            modelBuilder.Configurations.Add(new CategoryConfig());
            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new RoleConfig());
            modelBuilder.Configurations.Add(new CommentConfig());
            modelBuilder.Configurations.Add(new CalendarConfig());
        }
    }
}