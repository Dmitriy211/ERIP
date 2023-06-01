namespace ScienceKit;

public readonly struct ItemEntry: IEntry
{
    private readonly int _index;
    private readonly float _runTime;
    private readonly int _itemIndex;
    private readonly bool _isEquipment;
    private readonly bool _isAdded;
    
    private static string Columns => RowFormatter.FormatRow("", "ItemIndex", "IsEquipment", "IsAdded", "RunTime");
    string IEntry.Columns => Columns;

    public ItemEntry(int index, float runTime, int itemIndex, bool isEquipment, bool isAdded)
    {
        _index = index;
        _runTime = runTime;
        _itemIndex = itemIndex;
        _isEquipment = isEquipment;
        _isAdded = isAdded;
    }
    
    public override string ToString()
    {
        return RowFormatter.FormatRow($"{_index}", $"{_itemIndex}", $"{_isEquipment}", $"{_isAdded}", $"{_runTime}");
    }
}