using System;
using Xunit;
using CompanyCup2026.Formulas;

namespace CompanyCup2026.Tests.Tests
{
    public class CustomMathTests
    {
        [Fact]
        public void CalculateTargetSpeed_NegativeDistance_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                CustomMath.CalculateTargetSpeed(-0.1, 0, 0, 1, 1, 1));
        }

        [Fact]
        public void CalculateTargetSpeed_NonPositiveAccel_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                CustomMath.CalculateTargetSpeed(1, 0, 0, 0, 1, 1));
        }

        [Fact]
        public void CalculateTargetSpeed_NonPositiveDecel_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                CustomMath.CalculateTargetSpeed(1, 0, 0, 1, 0, 1));
        }

        [Fact]
        public void CalculateTargetSpeed_NonPositiveMaxSpeed_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                CustomMath.CalculateTargetSpeed(1, 0, 0, 1, 1, 0));
        }

        [Fact]
        public void CalculateTargetSpeed_BasicCalculation_ReturnsExpected()
        {
            // Parameters chosen so no clamp occurs and start/end speeds are zero.
            double dist = 100.0;
            double speedStart = 0.0;
            double speedEnd = 0.0;
            double accel = 2.0;
            double decel = 2.0;
            double maxSpeed = 1000.0; // large so it doesn't clamp

            var (targetSpeed, brakeDistance) = CustomMath.CalculateTargetSpeed(
                dist, speedStart, speedEnd, accel, decel, maxSpeed);

            // Expected by formula in the implementation:
            // targetSpeed^2 = (2*a*d*dist + d*vs^2 + a*ve^2)/(a+d)
            double expectedTargetSpeedSq = (2 * accel * decel * dist + decel * speedStart * speedStart + accel * speedEnd * speedEnd) / (accel + decel);
            double expectedTargetSpeed = Math.Sqrt(expectedTargetSpeedSq);
            double expectedAccelDistance = (expectedTargetSpeed * expectedTargetSpeed - speedStart * speedStart) / (2 * accel);

            Assert.Equal(expectedTargetSpeed, targetSpeed, 6);
            Assert.Equal(expectedAccelDistance, brakeDistance, 6);
        }

        [Fact]
        public void CalculateTargetSpeed_ClampsToMaxSpeed_ReturnsClampedAndCorrectDistance()
        {
            // Same setup as basic but force maxSpeed below computed target so clamp happens.
            double dist = 100.0;
            double speedStart = 0.0;
            double speedEnd = 0.0;
            double accel = 2.0;
            double decel = 2.0;
            double maxSpeed = 10.0; // small so clamp occurs

            var (targetSpeed, brakeDistance) = CustomMath.CalculateTargetSpeed(
                dist, speedStart, speedEnd, accel, decel, maxSpeed);

            Assert.Equal(maxSpeed, targetSpeed, 6);

            double expectedAccelDistance = (maxSpeed * maxSpeed - speedStart * speedStart) / (2 * accel);
            Assert.Equal(expectedAccelDistance, brakeDistance, 6);
        }

        [Fact]
        public void CalculateTargetSpeed_StartSpeedHigherThanTarget_AccelDistanceIsZero()
        {
            // Choose parameters so computed target < start speed, so accelDistance is clamped to 0.
            double dist = 1.0;
            double speedStart = 10.0;
            double speedEnd = 0.0;
            double accel = 2.0;
            double decel = 2.0;
            double maxSpeed = 1000.0;

            var (targetSpeed, brakeDistance) = CustomMath.CalculateTargetSpeed(
                dist, speedStart, speedEnd, accel, decel, maxSpeed);

            Assert.True(targetSpeed < speedStart);
            Assert.Equal(0.0, brakeDistance, 6);
        }
    }
}
