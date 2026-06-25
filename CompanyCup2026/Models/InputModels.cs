// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Text.Json.Serialization;

public static class RaceConstants
{
    public const double Gravity = 9.8;
    public const double K_STRAIGHT = 0.0000166;
    public const double K_BRAKING = 0.0398;
    public const double K_CORNER = 0.000265;
    public const double K_BASE_FUEL = 0.0005;
    public const double K_DRAG = 0.0000000015;
}

public class AvailableSet
{
    public List<int> ids { get; set; }
    public string compound { get; set; }
}

public class Car
{
    [JsonPropertyName("max_speed_m/s")]
    public int max_speed_ms { get; set; }

    [JsonPropertyName("accel_m/se2")]
    public int accel_mse2 { get; set; }

    [JsonPropertyName("brake_m/se2")]
    public int brake_mse2 { get; set; }

    [JsonPropertyName("limp_constant_m/s")]
    public int limp_constant_ms { get; set; }

    [JsonPropertyName("crawl_constant_m/s")]
    public int crawl_constant_ms { get; set; }
    public double fuel_tank_capacity_l { get; set; }
    public double initial_fuel_l { get; set; }

    [JsonPropertyName("fuel_consumption_l/m")]
    public double fuel_consumption_lm { get; set; }

    public double TimeToChangeSpeed(double initialSpeed, double finalSpeed)
    {
        return (finalSpeed - initialSpeed) / accel_mse2;
    }

    public double TimeToDecelerate(double initialSpeed, double finalSpeed)
    {
        return (finalSpeed - initialSpeed) / -brake_mse2;
    }

    public double DistanceFromSpeeds(double initialSpeed, double finalSpeed, double acceleration)
    {
        return (Math.Pow(finalSpeed, 2) - Math.Pow(initialSpeed, 2)) / (2 * acceleration);
    }

    public double DistanceFromTime(double initialSpeed, double time, double acceleration)
    {
        return (initialSpeed * time) + (0.5 * acceleration * Math.Pow(time, 2));
    }

    public double Speed(double distance, double time)
    {
        return distance / time;
    }

    public double FuelUsed(double initialSpeed, double finalSpeed, double distance)
    {
        double avgSpeed = (initialSpeed + finalSpeed) / 2.0;
        return (RaceConstants.K_BASE_FUEL + (RaceConstants.K_DRAG * Math.Pow(avgSpeed, 2))) * distance;
    }
}

public class Condition
{
    public int id { get; set; }
    public string condition { get; set; }
    public double duration_s { get; set; }
    public int acceleration_multiplier { get; set; }
    public int deceleration_multiplier { get; set; }
}

public class Properties
{
    public TyreProperties Soft { get; set; }
    public TyreProperties Medium { get; set; }
    public TyreProperties Hard { get; set; }
    public TyreProperties Intermediate { get; set; }
    public TyreProperties Wet { get; set; }
}

public class Race
{
    public string name { get; set; }
    public int laps { get; set; }
    public double base_pit_stop_time_s { get; set; }
    public double pit_tyre_swap_time_s { get; set; }

    [JsonPropertyName("pit_refuel_rate_l/s")]
    public double pit_refuel_rate_ls { get; set; }
    public double corner_crash_penalty_s { get; set; }

    [JsonPropertyName("pit_exit_speed_m/s")]
    public double pit_exit_speed_ms { get; set; }
    public double fuel_soft_cap_limit_l { get; set; }
    public int starting_weather_condition_id { get; set; }
    public double time_reference_s { get; set; }

    public double RefuelTime(double fuelAmount)
    {
        return fuelAmount / pit_refuel_rate_ls;
    }

    public double PitStopTime(double refuelTime)
    {
        return refuelTime + pit_tyre_swap_time_s + base_pit_stop_time_s;
    }

    public double FuelBonus(double fuelUsed)
    {
        double ratio = fuelUsed / fuel_soft_cap_limit_l;
        return (-1_000_000.0 * Math.Pow(1 - ratio, 2)) + 1_000_000.0;
    }

    public double BaseScore(double totalTime)
    {
        return 1_000_000_000.0 / totalTime;
    }

    public double TyreBonus(double totalDegradation, int blowouts)
    {
        return (100_000.0 * totalDegradation) - (50_000.0 * blowouts);
    }

    public double FinalScore(double totalTime, double fuelUsed, double tyreDeg, int blowouts)
    {
        return BaseScore(totalTime) + FuelBonus(fuelUsed) + TyreBonus(tyreDeg, blowouts);
    }
}

public class Root
{
    public Car car { get; set; }
    public Race race { get; set; }
    public Track track { get; set; }
    public Tyres tyres { get; set; }
    public List<AvailableSet> available_sets { get; set; }
    public Weather weather { get; set; }
    
    public List<TyreSet> GetTyreSets()
    {
        var tyreSets = new List<TyreSet>();
        
        foreach(var available_set in available_sets)
        {
            foreach(var tyreSetId in available_set.ids)
            {
                tyreSets.Add(new TyreSet(tyreSetId, available_set.compound, tyres.properties));
            }
        }

        return tyreSets;
    }
}

public class TyreSet
{
    public TyreSet (int id, string compound, Properties properties)
    {
        this.id = id;
        this.compound = compound;
        switch(compound)
        {
            case "Soft":
                this.TyreProperties = properties.Soft;
                break;
            case "Medium":
                this.TyreProperties = properties.Medium;
                break;
            case "Hard":
                this.TyreProperties = properties.Hard;
                break;
            case "Intermediate":
                this.TyreProperties = properties.Intermediate;
                break;
            case "Wet":
                this.TyreProperties = properties.Wet;
                break;
        }
    }

    public int id { get; set; }
    public string compound { get; set; }
    public TyreProperties TyreProperties{ get; set; }
}

public class Segment
{
    public int id { get; set; }
    public string type { get; set; }
    public int length_m { get; set; }
    public int? radius_m { get; set; }

    public double MaxCornerSpeed(double tyreFriction)
    {
        return Math.Sqrt(tyreFriction * RaceConstants.Gravity * radius_m.GetValueOrDefault());
    }

    public double MaxCornerSpeedWithCrawl(double tyreFriction, double crawlSpeed)
    {
        return Math.Sqrt((tyreFriction * RaceConstants.Gravity * radius_m.GetValueOrDefault()) + crawlSpeed);
    }
}

public class TyreProperties
{
    public int life_span { get; set; }
    public double base_friction { get; set; }
    public double dry_friction_multiplier { get; set; }
    public double cold_friction_multiplier { get; set; }
    public double light_rain_friction_multiplier { get; set; }
    public double heavy_rain_friction_multiplier { get; set; }
    public double dry_degradation { get; set; }
    public double cold_degradation { get; set; }
    public double light_rain_degradation { get; set; }
    public double heavy_rain_degradation { get; set; }

    public double TyreFriction(double totalDegradation, double weatherMultiplier)
    {
        return (base_friction - totalDegradation) * weatherMultiplier;
    }

    public double StraightDegradation(double degradationRate, double distance)
    {
        return degradationRate * distance * RaceConstants.K_STRAIGHT;
    }

    public double BrakingDegradation(double initialSpeed, double finalSpeed, double degradationRate)
    {
        double vi = initialSpeed / 100.0;
        double vf = finalSpeed / 100.0;
        return (Math.Pow(vi, 2) - Math.Pow(vf, 2)) * RaceConstants.K_BRAKING * degradationRate;
    }

    public double CornerDegradation(double speed, double radius, double degradationRate)
    {
        return RaceConstants.K_CORNER * Math.Pow(speed, 2) * radius * degradationRate;
    }
}

public class Track
{
    public string name { get; set; }
    public List<Segment> segments { get; set; }
}

public class Tyres
{
    public Properties properties { get; set; }
}

public class Weather
{
    public List<Condition> conditions { get; set; }
}
