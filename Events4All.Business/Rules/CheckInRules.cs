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
        public int IsValidCheckInTime(int eventId)
        {
            EventQuery eq = new EventQuery();
            EventDTO eDTO = new EventDTO();
            eDTO = eq.FindEvent(eventId);
            int isValidCode = 0;
            DateTime checkinStart = eDTO.TimeStart.Value.AddHours(-2);
            DateTime checkinEnd = eDTO.TimeStop.Value;

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

        public bool IsDuplicateCheckIn(string guid)
        {
            CheckInQuery ciq = new CheckInQuery();
            bool isDuplicate = ciq.IsDuplicateCheckIn(guid);
            return isDuplicate;
        }

    }
}
