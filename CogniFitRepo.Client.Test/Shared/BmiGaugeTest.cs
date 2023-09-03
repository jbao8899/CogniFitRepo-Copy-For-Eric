using CogniFitRepo.Client.Shared;
using NUnit;
using NUnit.Framework;
using System;

//writes a unit test class for the BmiGauge class
//the class should have a method that tests the BmiGauge class
//the class should have a method that tests the CalculateBMI method

namespace CogniFitRepo.Client.Test.Shared
{
    [TestFixture]
    public class BmiGaugeTest
    {
        [Test]
        public void TestBmiGauge()
        {
            //Arrange
            BmiGauge bmiGauge = new BmiGauge();
            //Act
            //Assert
            Assert.IsNotNull(bmiGauge);
        }

    }

[TestFixture]
    public class CalculateBMITests
    {
        [TestCase(60, 170, ExpectedResult = 20.76)] // 60kg and 170cm should result in a BMI of 20.76
        [TestCase(70, 180, ExpectedResult = 21.6)]  // 70kg and 180cm should result in a BMI of 21.6
        public double WhenCalled_ReturnsExpectedBMI(float weight, float height)
        {
            return BmiGauge.CalculateBMI(weight, height); 
        }

        [Test]
        public void GivenZeroHeight_ThrowsException()
        {
            // Trying to calculate BMI with zero height should cause division by zero, 
            // which would throw an exception. This test ensures that such an exception is thrown.
            Assert.Throws<System.DivideByZeroException>(() => BmiGauge.CalculateBMI(70, 0));
        }

        [Test]
        public void GivenNegativeHeight_ThrowsException()
        {
            // Negative values for height (or weight) are not physiologically meaningful
            Assert.Throws<DivideByZeroException>(() => BmiGauge.CalculateBMI(70, -170));
        }

       
    }

}


