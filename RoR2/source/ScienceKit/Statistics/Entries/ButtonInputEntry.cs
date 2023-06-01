namespace ScienceKit
{
    public readonly struct ButtonInputEntry: IEntry
    {
        private readonly int _index;
        private readonly string _actionName;
        private readonly State _state;
        private readonly float _runTime;
        
        private static string Columns => RowFormatter.FormatRow("", "ActionName", "State", "RunTime");
        string IEntry.Columns => Columns;
        

        public ButtonInputEntry(int index, float runTime, string actionName, State state)
        {
            _index = index;
            _actionName = actionName;
            _state = state;
            _runTime = runTime;
        }

        public override string ToString()
        {
            return RowFormatter.FormatRow($"{_index}", _actionName, $"{_state}", $"{_runTime}");
        }

        public enum State
        {
            Released = 0,
            Pressed = 1
        }
    }
}