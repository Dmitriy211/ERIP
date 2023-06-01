namespace ScienceKit;

public static class RowFormatter
{
    private const string ENTRY_DELIMITER = ",";
    private const string ROW_DELIMITER = "";
    
    public static string FormatRow(params string[] entries)
    {
        if (entries.Length == 0) return $"{ROW_DELIMITER}";
        
        string row = "";
        
        for (int i = 0; i < entries.Length - 1; i++) 
            row += $"{entries[i]}{ENTRY_DELIMITER}";
        row += $"{entries[entries.Length - 1]}{ROW_DELIMITER}";

        return row;
    }
}