using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest
{
    public class WorkDayCalculator : IWorkDayCalculator
    {
        public DateTime Calculate(DateTime startDate, int dayCount, WeekEnd[] weekEnds)
        {            
            if (weekEnds == null)
            {
                return startDate.AddDays(dayCount - 1);
			}

			// filter for weekends that are contained in work interval
			var weekEndsFiltered = weekEnds.Where(wknd => DateTime.Compare(wknd.EndDate, startDate) >= 0
                                               && DateTime.Compare(wknd.StartDate, startDate.AddDays(dayCount - 1)) <= 0);
			// if weekend beginning before work interval - cut it

            foreach (var wknd in weekEndsFiltered)
            {
                if (DateTime.Compare(wknd.StartDate, startDate) < 0)
                {
					wknd.StartDate = startDate;
				}
            }

            // calculate number of weekends 
			var weekEndsTotalDays = 0;
			foreach (var weekEnd in weekEndsFiltered)
            {
                var dif = (int)(weekEnd.EndDate - weekEnd.StartDate).TotalDays;
                weekEndsTotalDays += dif >= 0 ? dif + 1 : 0;
            }

            return startDate.AddDays(dayCount - 1 + weekEndsTotalDays);
        }
    }
}
