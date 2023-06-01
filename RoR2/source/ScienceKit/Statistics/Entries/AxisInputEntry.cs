namespace ScienceKit;

public readonly struct AxisInputEntry: IEntry
{
    private readonly int _index;
    private readonly string _actionName;
    private readonly float _delta;
    private readonly float _runTime;

    private static string Columns => RowFormatter.FormatRow("", "ActionName", "Delta", "RunTime");
    string IEntry.Columns => Columns;

    public AxisInputEntry(int index, float runTime, string actionName, float delta)
    {
        _index = index;
        _actionName = actionName;
        _delta = delta;
        _runTime = runTime;
    }

    public override string ToString()
    {
        return RowFormatter.FormatRow($"{_index}", _actionName, $"{_delta}", $"{_runTime}");
    }
}