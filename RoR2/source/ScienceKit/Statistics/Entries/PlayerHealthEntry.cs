namespace ScienceKit;

public readonly struct PlayerHealthEntry: IEntry
{
    private readonly int _index;
    private readonly float _runTime;
    private readonly float _health;
    private readonly float _healthFraction;
    
    private static string Columns => RowFormatter.FormatRow("", "Health", "HealthFraction", "RunTime");
    string IEntry.Columns => Columns;
    
    public PlayerHealthEntry(int index, float runTime, float health, float healthFraction)
    {
        _index = index;
        _runTime = runTime;
        _health = health;
        _healthFraction = healthFraction;
    }

    public override string ToString()
    {
        return RowFormatter.FormatRow($"{_index}", $"{_health}", $"{_healthFraction}", $"{_runTime}");
    }
}