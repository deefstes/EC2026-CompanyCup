using CompanyCup2026.Formulas;

public class Solver
{
    public OutputRoot SolverLevel1(Root inputRoot)
    {
        var outputRoot = new OutputRoot();
        var tyreSets = inputRoot.GetTyreSets();
        var carSpeed = (double)0;

        var currentTyreSet = tyreSets.First();
        outputRoot.SetInitialTyreSet(currentTyreSet);

        for(var lapNo = 0; lapNo < inputRoot.race.laps; lapNo++)
        {
            var outputLap = outputRoot.AddLap();

            for (var i=0; i<inputRoot.track.segments.Count; i++)
            {
                var endSpeed = inputRoot.track.segments[(i + 1) % inputRoot.track.segments.Count].MaxCornerSpeed(currentTyreSet.TyreProperties.base_friction);

                var inputSegment = inputRoot.track.segments[i];
                var outputSegment = outputLap.AddSegment(inputSegment);
                if (outputSegment.type == "corner")
                {
                    continue;
                }

                var (targetSpeed, brakePoint) = CustomMath.CalculateTargetSpeed(
                    inputSegment.length_m,
                    carSpeed,
                    endSpeed,
                    inputRoot.car.accel_mse2,
                    inputRoot.car.brake_mse2,
                    inputRoot.car.max_speed_ms);

                outputSegment.target_ms = (int)targetSpeed;
                outputSegment.brake_start_m_before_next = (int)brakePoint;
                
                outputSegment.SetFuelTrackingProperties(carSpeed, endSpeed, inputSegment.length_m);

                carSpeed = endSpeed;
            }

            outputLap.SetNoPit();
        }        

        return outputRoot;
    }
}