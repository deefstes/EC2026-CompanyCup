// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Text.Json.Serialization;

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
