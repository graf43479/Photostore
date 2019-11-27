using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ICalendarRepository
    {
        IQueryable<Calendar> Calendars { get; }

        void SaveCalendar(Calendar calendar);

        void DeleteCalendar(Calendar calendar);

    }
}
