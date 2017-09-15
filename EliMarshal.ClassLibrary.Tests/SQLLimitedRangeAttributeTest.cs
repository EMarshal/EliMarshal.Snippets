namespace EliMarshal.ClassLibrary.Tests
{
    using Xunit;

    public class SQLLimitedRangeAttributeTest
    {
        [Fact]
        public void ShouldValidateMoneyDecimal()
        {
            SQLLimitedRangeTestClass test = new SQLLimitedRangeTestClass
            {
                TestMoneyDecimal = 0
            };
            Assert.True(test.IsValid("TestMoneyDecimal"));

            test.TestMoneyDecimal = 9999999.99M;
            Assert.True(test.IsValid("TestMoneyDecimal"));

            test.TestMoneyDecimal = -9999999.99M;
            Assert.True(test.IsValid("TestMoneyDecimal"));

            test.TestMoneyDecimal = 10000000.00M;
            Assert.False(test.IsValid("TestMoneyDecimal"));

            test.TestMoneyDecimal = -10000000.00M;
            Assert.False(test.IsValid("TestMoneyDecimal"));
        }

        [Fact]
        public void ShouldValidatePercentDecimal()
        {
            SQLLimitedRangeTestClass test = new SQLLimitedRangeTestClass
            {
                TestPercentDecimal = 0
            };
            Assert.True(test.IsValid("TestPercentDecimal"));

            test.TestPercentDecimal = 9999.99999M;
            Assert.True(test.IsValid("TestPercentDecimal"));

            test.TestPercentDecimal = -9999.99999M;
            Assert.True(test.IsValid("TestPercentDecimal"));

            test.TestPercentDecimal = 10000.00000M;
            Assert.False(test.IsValid("TestPercentDecimal"));

            test.TestPercentDecimal = -10000.00000M;
            Assert.False(test.IsValid("TestPercentDecimal"));
        }

        private class SQLLimitedRangeTestClass : ModelBase<SQLLimitedRangeAttribute>
        {
            [SQLLimitedRange(9, 2)]
            public decimal TestMoneyDecimal { get; set; }

            [SQLLimitedRange(9, 5)]
            public decimal TestPercentDecimal { get; set; }
        }
    }
}
