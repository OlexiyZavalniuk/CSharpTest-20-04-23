using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest
{
    [TestClass]
    public class WorkDayCalculatorTests
    {

		//   |------| - work interval
		//   XX - weekends
		//   
		//    We have 5 variants     Result:
		//    of combination:
		//
		//    1) XX |------|         t
		//    2) X|X-----|           t + X
		//    3) |--XX--|            t + XX
		//    4) |-----X|X           t + XX
		//    5) |------| XX         t


		[TestMethod]
        public void TestNoWeekEnd()
        {
            DateTime startDate = new DateTime(2021, 12, 1);
            int count = 10;

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, null);

            Assert.AreEqual(startDate.AddDays(count-1), result);
        }

		// tests combination 3
		[TestMethod]
        public void TestNormalPath()
        {
            DateTime startDate = new DateTime(2021, 4, 21);
            int count = 5;
            WeekEnd[] weekends = new WeekEnd[1]
            {
                new WeekEnd(new DateTime(2021, 4, 23), new DateTime(2021, 4, 25))
            }; 

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.IsTrue(result.Equals(new DateTime(2021, 4, 28)));
        }

		// tests combination 3 and 5
		[TestMethod]
        public void TestWeekendAfterEnd()
        {
            DateTime startDate = new DateTime(2021, 4, 21);
            int count = 5;
            WeekEnd[] weekends = new WeekEnd[2]
            {
                new WeekEnd(new DateTime(2021, 4, 23), new DateTime(2021, 4, 25)),
                new WeekEnd(new DateTime(2021, 4, 29), new DateTime(2021, 4, 29))
            };
            
            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.IsTrue(result.Equals(new DateTime(2021, 4, 28)));
        }

		// tests combination 1 and 4
		[TestMethod]
        public void TestWeekendBeforeStart()
        {
			DateTime startDate = new DateTime(2021, 4, 21);
			int count = 5;
			WeekEnd[] weekends = new WeekEnd[2]
			{
				new WeekEnd(new DateTime(2021, 4, 25), new DateTime(2021, 4, 27)),
				new WeekEnd(new DateTime(2021, 4, 10), new DateTime(2021, 4, 14))
			};

			DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

			Assert.IsTrue(result.Equals(new DateTime(2021, 4, 28)));
		}

		// tests combination 2
		[TestMethod]
		public void TestWeekendStartBeforeStart()
		{
			DateTime startDate = new DateTime(2021, 4, 21);
			int count = 5;
			WeekEnd[] weekends = new WeekEnd[1]
			{
				new WeekEnd(new DateTime(2021, 4, 19), new DateTime(2021, 4, 22))
			};

			DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

			Assert.IsTrue(result.Equals(new DateTime(2021, 4, 27)));
		}

		// combination X------X when weekends on border of time interval
		[TestMethod]
		public void TestWeekendInStartAndInEnd()
		{
			DateTime startDate = new DateTime(2021, 4, 21);
			int count = 5;
			WeekEnd[] weekends = new WeekEnd[2]
			{
				new WeekEnd(new DateTime(2021, 4, 21), new DateTime(2021, 4, 21)),
				new WeekEnd(new DateTime(2021, 4, 25), new DateTime(2021, 4, 25))
			};

			DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

			Assert.IsTrue(result.Equals(new DateTime(2021, 4, 27)));
		}
	}
}
