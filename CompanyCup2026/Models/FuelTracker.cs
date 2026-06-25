public class FuelTracker
{
    public FuelTracker(double initial_fuel_l, double fuel_consumption_lm, double fuel_tank_capacity_l)
    {
        this.initial_fuel_l = initial_fuel_l;
        this.fuel_consumption_lm = fuel_consumption_lm;
        this.fuel_tank_capacity_l = fuel_tank_capacity_l;
    }
    private double initial_fuel_l { get; set; }
    private double fuel_consumption_lm { get; set; }
    public double fuel_tank_capacity_l { get; set; }

    public double CalculateFuelUsage(double initialSpeed, double finalSpeed, int distance)
    {
        var averageSpeed = (initialSpeed + finalSpeed)/2;
        var averageSpeedSquared = averageSpeed * averageSpeed;
        return (fuel_consumption_lm + RaceConstants.K_DRAG*averageSpeedSquared)*distance;
    }

    public void SetRefuelPitStop(OutputRoot outputRoot)
    {
        var currentFuelCapacity = initial_fuel_l;

        var carSpeed = (double)0;

        for(var lapNo = 0; lapNo < outputRoot.laps.Count; lapNo++)
        {
            var lap = outputRoot.laps[lapNo];

            for (var i=0; i < lap.segments.Count; i++)
            {
                var endSpeed = inputRoot.track.segments[(i + 1) % inputRoot.track.segments.Count].MaxCornerSpeed(currentTyreSet.TyreProperties.base_friction);


                carSpeed = endSpeed;
            }

            outputLap.SetNoPit();
        }        
    }
}