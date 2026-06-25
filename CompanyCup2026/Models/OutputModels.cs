// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Text.Json.Serialization;

public class OutputLap
{
    public int lap { get; set; } = 1;
    public List<OutputSegment> segments { get; set; } = new List<OutputSegment>();
    public OutputPit pit { get; set; }
    public OutputSegment AddSegment(Segment inputSegment)
    {
        var segment = new OutputSegment();

        segment.id = inputSegment.id;
        segment.type = inputSegment.type;

        segments.Add(segment);

        return segment;
    }
    public OutputPit SetNoPit()
    {
        this.pit = new OutputPit();
        this.pit.enter = false;

        return this.pit;
    }

    public OutputPit SetPit(int fuel_refuel_amount_l,TyreSet tyreSet)
    {
        this.pit = new OutputPit();
        this.pit.enter = true;
        this.pit.tyre_change_set_id = tyreSet.id;
        this.pit.fuel_refuel_amount_l = fuel_refuel_amount_l;
        
        return this.pit;
    }
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
    public List<OutputLap> laps { get; set; } = new List<OutputLap>();
    public void SetInitialTyreSet(TyreSet tyreSet)
    {
        this.initial_tyre_id = tyreSet.id;
    }
    
    public OutputLap AddLap()
    {
        var outputLap = new OutputLap();

        if (laps.Count > 0)
        {
            outputLap.lap = laps.Last().lap + 1;
        }

        laps.Add(outputLap);

        return outputLap;
    }
}

public class OutputSegment
{
    public int id { get; set; }
    public string type { get; set; }

    [JsonPropertyName("target_m/s")]
    public int target_ms { get; set; }
    public int brake_start_m_before_next { get; set; }

    public void SetFuelTrackingProperties(double startSpeed, double endSpeed, int length)
    {
        this.startSpeed = startSpeed;
        this.endSpeed = endSpeed;
        this.length = length;
    }

    [JsonIgnore]
    public double startSpeed { get; set; }
    [JsonIgnore]
    public double endSpeed { get; set; }
    [JsonIgnore]
    public int length { get; set; }
}

