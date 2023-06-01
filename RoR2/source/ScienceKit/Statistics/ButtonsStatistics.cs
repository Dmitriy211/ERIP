using System.Collections.Generic;
using System.Linq;
using Rewired;

namespace ScienceKit;

public class ButtonsStatistics : InputStatistics<ButtonInputEntry>
{
    protected override StatisticType statisticType => StatisticType.ButtonInputs;
    
    private readonly string[] _observedButtons = 
    {
        "Jump",
        "Sprint",
        "Interact",
        "Equipment",
        "PrimarySkill",
        "SecondarySkill",
        "UtilitySkill",
        "SpecialSkill",
        "Info",
        "Ping"
    };
    
    protected override void LogInputStatistic(Player inputPlayer)
    {
        var buttonsDown = new Dictionary<string, bool>();
        var buttonsUp = new Dictionary<string, bool>();
            
        foreach (var action in _observedButtons)
        {
            buttonsDown[action] = inputPlayer.GetButtonDown(action);
            buttonsUp[action] = inputPlayer.GetButtonUp(action);
        }

        if (buttonsDown.Count == 0 || buttonsUp.Count == 0) return;
        foreach (var pair in buttonsDown.Where(pair => pair.Value == true))
            Inputs.Add(new ButtonInputEntry(Inputs.Count, currentRun.time, pair.Key, ButtonInputEntry.State.Pressed));
        foreach (var pair in buttonsUp.Where(pair => pair.Value == true))
            Inputs.Add(new ButtonInputEntry(Inputs.Count, currentRun.time, pair.Key, ButtonInputEntry.State.Released));
    }
}