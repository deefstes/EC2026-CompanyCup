public static class RaceMath
{

    // CONSTANTS

    public const double Gravity = 9.8;

    // Tyre degradation constants
    public const double K_STRAIGHT = 0.0000166;
    public const double K_BRAKING = 0.0398;
    public const double K_CORNER = 0.000265;

    // Fuel constants
    public const double K_BASE_FUEL = 0.0005;
    public const double K_DRAG = 0.0000000015;


    // KINEMATICS


    // Time to accelerate/decelerate between speeds 
    public static double TimeToChangeSpeed(double initialSpeed, double finalSpeed, double acceleration)
    {
        return (finalSpeed - initialSpeed) / acceleration;
    }

    // Distance when final speed is known 
    public static double DistanceFromSpeeds(double initialSpeed, double finalSpeed, double acceleration)
    {
        return (Math.Pow(finalSpeed, 2) - Math.Pow(initialSpeed, 2)) / (2 * acceleration);
    }

    // Distance when time is known 
    public static double DistanceFromTime(double initialSpeed, double time, double acceleration)
    {
        return (initialSpeed * time) + (0.5 * acceleration * Math.Pow(time, 2));
    }

    // Speed from distance and time 
    public static double Speed(double distance, double time)
    {
        return distance / time;
    }



    // CORNERING


    // Max corner speed 
    public static double MaxCornerSpeed(double tyreFriction, double radius)
    {
        return Math.Sqrt(tyreFriction * Gravity * radius);
    }

    // Variant including crawl constant 
    public static double MaxCornerSpeedWithCrawl(double tyreFriction, double radius, double crawlSpeed)
    {
        return Math.Sqrt((tyreFriction * Gravity * radius) + crawlSpeed);
    }



    // TYRE CALCULATIONS


    // Tyre friction 
    public static double TyreFriction(double baseFriction, double totalDegradation, double weatherMultiplier)
    {
        return (baseFriction - totalDegradation) * weatherMultiplier;
    }

    // Straight degradation 
    public static double StraightDegradation(double degradationRate, double distance)
    {
        return degradationRate * distance * K_STRAIGHT;
    }

    // Braking degradation 
    public static double BrakingDegradation(double initialSpeed, double finalSpeed, double degradationRate)
    {
        double vi = initialSpeed / 100.0;
        double vf = finalSpeed / 100.0;

        return (Math.Pow(vi, 2) - Math.Pow(vf, 2)) * K_BRAKING * degradationRate;
    }

    // Corner degradation 
    public static double CornerDegradation(double speed, double radius, double degradationRate)
    {
        return K_CORNER * Math.Pow(speed, 2) * radius * degradationRate;
    }



    // FUEL


    // Fuel used over a segment 
    public static double FuelUsed(double initialSpeed, double finalSpeed, double distance)
    {
        double avgSpeed = (initialSpeed + finalSpeed) / 2.0;

        return (K_BASE_FUEL + (K_DRAG * Math.Pow(avgSpeed, 2))) * distance;
    }

    // Refuel time 
    public static double RefuelTime(double fuelAmount, double refuelRate)
    {
        return fuelAmount / refuelRate;
    }



    // PIT STOP


    // Total pit time 
    public static double PitStopTime(double refuelTime, double tyreSwapTime, double basePitTime)
    {
        return refuelTime + tyreSwapTime + basePitTime;
    }



    // SCORING


    // Base score 
    public static double BaseScore(double totalTime)
    {
        return 1_000_000_000.0 / totalTime;
    }

    // Fuel bonus 
    public static double FuelBonus(double fuelUsed, double fuelSoftCap)
    {
        double ratio = fuelUsed / fuelSoftCap;
        return (-1_000_000.0 * Math.Pow(1 - ratio, 2)) + 1_000_000.0;
    }

    // Tyre bonus 
    public static double TyreBonus(double totalDegradation, int blowouts)
    {
        return (100_000.0 * totalDegradation) - (50_000.0 * blowouts);
    }

    // Final score (level 4)
    public static double FinalScore(double totalTime, double fuelUsed, double fuelSoftCap, double tyreDeg, int blowouts)
    {
        return BaseScore(totalTime) + FuelBonus(fuelUsed, fuelSoftCap) + TyreBonus(tyreDeg, blowouts);
    }
}