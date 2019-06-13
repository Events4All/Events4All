using Events4All.DBQuery;
using Events4All.DBQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.Business.Rules
{
    public class CheckInRules
    {
        public int IsValidCheckInTime(DateTime[] eventTimes)
        {
            int isValidCode = 0;

            DateTime checkinStart = eventTimes[0].AddHours(-2);
            DateTime checkinEnd = eventTimes[1];

            if (DateTime.Now < checkinStart)
            {
                isValidCode = -1;
            }
            else if (DateTime.Now > checkinEnd)
            {
                isValidCode = 1;
            }

            return isValidCode;
        }
        
        public bool IsDuplicateCheckIn(List<DateTime> checkInTimes)
        {
            bool isDuplicateCheckIn = false;
            foreach(DateTime checkInTime in checkInTimes)
            {
                if(checkInTime.Day == DateTime.Now.Day)
                {
                    isDuplicateCheckIn = true;
                }
            }

            return isDuplicateCheckIn;
        }
    }
}
