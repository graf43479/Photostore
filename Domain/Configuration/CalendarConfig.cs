using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Configuration
{
    public class CalendarConfig : EntityTypeConfiguration<Calendar>
    {
        public CalendarConfig()
        {
            HasKey(p => p.CalendarID);
            Property(p => p.CalendarID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.CalendarDate).HasColumnType("datetime2");
        }
    }
}
