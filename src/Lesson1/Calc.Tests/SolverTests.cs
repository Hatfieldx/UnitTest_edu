using Xunit;

namespace Calc.Tests
{    
    public class SolverTests
    {
        [Fact]
        public void Sum_5_4_Res9()
        {
            //Arang
            double x = 5;
            double y = 4;

            //Act

            double res = Solver.Sum(x, y);

            //Acert

            Assert.Equal(9, res);
        }
    }
}
