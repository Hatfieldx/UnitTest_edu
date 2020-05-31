using System;
using Xunit;

namespace AssertSamples.Tests
{
    public class AssertBaseMethods
    {
        [Fact]
        public void SqrtTest()
        {
            // arrange
            const double input = 4;
            const double expected = 2;

            //act
            var res = MyClass.GetSqrt(4);

            //arrange

            Assert.Equal(expected, res);
        }

        [Fact]
        public void DeltaTest()
        {
            const double expected = 3.1;
            const double delta = 0.07;

            // 3.1622776601683795
            // 0.062..
            double actual = MyClass.GetSqrt(10);

            // Проверка значений на равенство с учетом погрешности delta
            Assert.Equal(expected, actual, 0);

        }

        [Fact]
        public void StringAreEqualTest()
        {
            // arrange
            const string input = "HELLO";
            const string expected = "hello";

            // act and assert
            // третий параметр - игнорирование регистра
            Assert.Equal(expected, input, ignoreCase: true);

        }

        [Fact]
        public void StringSameTest()
        {
            string a = "Hello";
            string b = "Hello";

            // проверка равенства ссылок
            Assert.Same(a, b);
        }

        [Fact(Skip = "doesnt work with value types")]
        public void IntegersSameTest()
        {
            int i = 10;
            int j = 10;

            // проверка равенства ссылок
            Assert.Same(i, j);
        }
    }
}
