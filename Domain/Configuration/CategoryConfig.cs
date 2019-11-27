using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Domain.Configuration
{
    public class CategoryConfig : EntityTypeConfiguration<Category>
    {
        public CategoryConfig()
        {
            HasKey(p => p.CategoryID);
            HasRequired(p => p.Products);
            Property(p => p.CategoryID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.UpdateDate).HasColumnType("datetime2");
            Property(p => p.CreateDate).HasColumnType("datetime2");
        }
    }
}