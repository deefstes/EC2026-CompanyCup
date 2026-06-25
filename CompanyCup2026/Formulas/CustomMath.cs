namespace CompanyCup2026.Formulas
{
    public static class CustomMath
    {
        public static (double targetSpeed, double brakeDistance) CalculateTargetSpeed(
            double dist,
            double speedStart,
            double speedEnd,
            double accel,
            double decel,
            double maxSpeed)
        {
            if (dist < 0)
                throw new ArgumentOutOfRangeException(nameof(dist));

            if (accel <= 0)
                throw new ArgumentOutOfRangeException(nameof(accel));

            if (decel <= 0)
                throw new ArgumentOutOfRangeException(nameof(decel));

            if (maxSpeed <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxSpeed));

            // Peak speed achievable if there is no speed limit.
            double targetSpeedSquared =
                (2 * accel * decel * dist
                 + decel * speedStart * speedStart
                 + accel * speedEnd * speedEnd)
                / (accel + decel);

            double targetSpeed = Math.Sqrt(Math.Max(0.0, targetSpeedSquared));

            // Clamp to the speed limit.
            if (targetSpeed > maxSpeed)
                targetSpeed = maxSpeed;

            // Distance at which braking should be applied
            double brakingDistance = Math.Max(0.0, (targetSpeed * targetSpeed - speedEnd * speedEnd) / (2 * decel));

            double brakePoint = Math.Max(0.0, dist - brakingDistance);

            return (targetSpeed, brakePoint);
        }
    }
}
