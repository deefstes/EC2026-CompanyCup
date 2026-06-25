public class Solver
{
    public OutputRoot SolverLevel1(Root inputRoot)
    {
        var outputRoot = new OutputRoot();
        var tyreSets = inputRoot.GetTyreSets();
        
        outputRoot.SetInitialTyreSet(tyreSets.First());

        for(var lapNo = 0; lapNo < inputRoot.race.laps; lapNo++)
        {
            var outputLap = outputRoot.AddLap();

            foreach(var inputSegment in inputRoot.track.segments)
            {
                var outputSegment = outputLap.AddSegment(inputSegment);
            }

            outputLap.SetNoPit();
        }
        

        return outputRoot;
    }
}