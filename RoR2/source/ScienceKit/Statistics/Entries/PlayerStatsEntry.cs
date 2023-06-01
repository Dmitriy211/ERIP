namespace ScienceKit;

public class PlayerStatsEntry: IEntry
{
    private readonly int _index;
    private readonly float _runTime;
    private readonly float _averageSpeed;
    private readonly float _dps;
    
    private static string Columns => RowFormatter.FormatRow("", "AverageSpeed", "DPS", "RunTime");
    string IEntry.Columns => Columns;

    public PlayerStatsEntry(int index, float runTime, float averageSpeed, float dps)
    {
        _index = index;
        _runTime = runTime;
        _averageSpeed = averageSpeed;
        _dps = dps;
    }
    
    public override string ToString()
    {
        return RowFormatter.FormatRow($"{_index}", $"{_averageSpeed}", $"{_dps}", $"{_runTime}");
    }
}