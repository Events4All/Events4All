using Events4All.Business.Rules;
using Events4All.DB.Models;
using Events4All.DBQuery;
using Events4All.DBQuery.DTO;
using Events4All.DBQuery.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Events4All.Tests
{
    [TestClass]
    public class CheckInRulesTest
    {
        [TestMethod]
        public void IsValidCheckInTimeTest()
        {
            CheckInRules ciRules = new CheckInRules();

            DateTime[] checkinMoreThanTwoHoursPriorToStart = { DateTime.Now.AddHours(3), DateTime.Now.AddHours(4) };
            DateTime[] checkinWithinTwoHoursPriorToStart = { DateTime.Now.AddHours(1), DateTime.Now.AddHours(2) };
            DateTime[] checkinAfterStartBeforeEnd = { DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1) };
            DateTime[] checkinAfterEnd = { DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1) };

            Assert.AreEqual(-1, ciRules.IsValidCheckInTime(checkinMoreThanTwoHoursPriorToStart));
            Assert.AreEqual(0, ciRules.IsValidCheckInTime(checkinWithinTwoHoursPriorToStart));
            Assert.AreEqual(0, ciRules.IsValidCheckInTime(checkinAfterStartBeforeEnd));
            Assert.AreEqual(1, ciRules.IsValidCheckInTime(checkinAfterEnd));

        }

        [TestMethod]
        public void IsDuplicateCheckInTest()
        {
            CheckInRules ciRules = new CheckInRules();

            List<DateTime> noCheckIns = new List<DateTime>();
            List<DateTime> checkInYesterday = new List<DateTime>(){ DateTime.Now.AddDays(-1) };
            List<DateTime> checkInToday = new List<DateTime>() { DateTime.Now };

            Assert.AreEqual(false, ciRules.IsDuplicateCheckIn(noCheckIns));
            Assert.AreEqual(false, ciRules.IsDuplicateCheckIn(checkInYesterday));
            Assert.AreEqual(true, ciRules.IsDuplicateCheckIn(checkInToday));
        }
    }
}
