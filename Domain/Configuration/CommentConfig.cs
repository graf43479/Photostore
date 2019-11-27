using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Configuration
{
    public class CommentConfig: EntityTypeConfiguration<Comment>
    {
        public CommentConfig()
        {
            HasKey(p => p.CommentID);
            Property(p => p.CommentID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.CreateDate).HasColumnType("datetime2");
        }
    }
}
