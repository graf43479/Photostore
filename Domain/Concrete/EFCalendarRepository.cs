using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFCalendarRepository : ICalendarRepository
    {
        private PhotoDBContext context;

        public EFCalendarRepository(PhotoDBContext context)
        {
            this.context = context;
        }

         public IQueryable<Calendar> Calendars
         {
             get { return context.Calendars; } 
            }

         public void SaveCalendar(Calendar calendar)
        {
            if (calendar.CalendarID == 0)
            {
                context.Calendars.Add(calendar);
            }
            else
            {
                context.Entry(calendar).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

         public void DeleteCalendar(Calendar calendar)
        {
            context.Calendars.Remove(calendar);
            context.SaveChanges();
        }
    }
}
