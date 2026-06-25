// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Text.Json.Serialization;

public class OutputLap
{
    public int lap { get; set; }
    public List<OutputSegment> segments { get; set; }
    public OutputPit pit { get; set; }
}

public class OutputPit
{
    public bool enter { get; set; }
    public int? tyre_change_set_id { get; set; }
    public int? fuel_refuel_amount_l { get; set; }
}

public class OutputRoot
{
    public int initial_tyre_id { get; set; }
    public List<OutputLap> laps { get; set; }
}

public class OutputSegment
{
    public int id { get; set; }
    public string type { get; set; }

    [JsonPropertyName("target_m/s")]
    public int target_ms { get; set; }
    public int brake_start_m_before_next { get; set; }
}

