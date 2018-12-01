using System;
using Xunit;

namespace Utilities.UnitTests
{
    public class PeriodTests
    {
        private const string JANUARY_ON_RUSSIAN_LOWER_CASE = "январь";

        private const string STRING_YEAR = "2018";

        [Fact]
        public void ToString_ShouldContainsMonthAndYearOnRussian()
        {
            //arrange
            Period period = new Period() {Month = 1, Year = DateTime.Now.Year };

            //act
            string toStingValue = period.ToString().ToLower();

            //assert
            Assert.Contains(JANUARY_ON_RUSSIAN_LOWER_CASE, toStingValue);
        }
    }
}
