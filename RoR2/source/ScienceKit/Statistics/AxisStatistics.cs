using System.Collections.Generic;
using System.Linq;
using Rewired;

namespace ScienceKit;

public class AxisStatistics: InputStatistics<AxisInputEntry>
{
    protected override StatisticType statisticType => StatisticType.AxisInputs;

    private readonly string[] _observedAxis = 
    {
        "MoveHorizontal",
        "MoveVertical",
        "AimHorizontalMouse",
        "AimVerticalMouse",
        "AimHorizontalStick",
        "AimVerticalStick"
    };

    protected override void LogInputStatistic(Player inputPlayer)
    {
        var axisValues = new Dictionary<string, float>();
        foreach (var action in _observedAxis)
        {
            axisValues[action] = inputPlayer.GetAxisRawDelta(action);
        }
            
        if (axisValues.Count == 0) return;
        foreach (var pair in axisValues.Where(pair => pair.Value != 0))
            Inputs.Add(new AxisInputEntry(Inputs.Count, currentRun.time, pair.Key, pair.Value));
    }
}