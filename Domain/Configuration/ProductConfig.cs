using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Domain.Configuration
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public ProductConfig()
        {
            HasKey(p => p.ProductID);
            HasRequired(p => p.Category).WithMany(x=>x.Products).HasForeignKey(m=>m.CategoryID).WillCascadeOnDelete(true);
            Property(p => p.ProductID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Property(p => p.ImgExt).HasMaxLength(4).IsOptional();
            

        }
    }
}