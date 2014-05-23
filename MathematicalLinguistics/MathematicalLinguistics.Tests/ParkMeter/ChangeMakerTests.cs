using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MathematicalLinguistics.Tests.ParkMeter
{
    public class ChangeMakerTests
    {
        ChangeMaker _changeMaker = new ChangeMaker(new int[]{
            100, 200, 500
        });

        [Fact]
        public void NoChange()
        {
            var change = _changeMaker.Make(10, 10);
            Assert.Equal(new int[] { 0 }, change);
        }

        [Fact(Skip="")]
        public void Example()
        {
            var change = _changeMaker.Make(100, 113);
            Assert.Equal(new int[] { 500, 500, 200, 100 }, change);
        }
    }
}
