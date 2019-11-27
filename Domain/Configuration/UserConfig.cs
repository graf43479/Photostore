using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Domain.Configuration
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            HasKey(p => p.UserID);
            HasRequired(p => p.Role).WithMany(x => x.Users).HasForeignKey(m => m.RoleID).WillCascadeOnDelete(true);
            Property(p => p.UserID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
}