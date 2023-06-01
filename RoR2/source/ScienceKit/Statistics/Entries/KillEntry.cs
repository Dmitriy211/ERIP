using RoR2;
using UnityEngine;

namespace ScienceKit;

public readonly struct KillEntry: IEntry
{
    private readonly int _index;
    private readonly int _entityID;
    private readonly string _entityName;
    private readonly float _level;
    private readonly float _distanceToPlayer;
    private readonly float _runTime;
    
    private static string Columns => RowFormatter.FormatRow("", "EntityID", "EntityName", "Level", "DistanceToPlayer", "RunTime");
    string IEntry.Columns => Columns;

#pragma warning disable Publicizer001
    public KillEntry(int index, float runTime, DamageReport damageReport)
    {
        var playerTransform = PlayerCharacterMasterController.instances[0].body.coreTransform;
        if (playerTransform == null) return;
            
        _index = index;
        _entityID = damageReport.victimBody.GetHashCode();
        _entityName = damageReport.victimBody.baseNameToken;
        _level = damageReport.victimBody.level;
        _distanceToPlayer = Vector3.Distance(damageReport.victimBody.corePosition, playerTransform.position);
        _runTime = runTime;
    }
#pragma warning restore Publicizer001

    public override string ToString()
    {
        return RowFormatter.FormatRow($"{_index}", $"{_entityID}", _entityName, $"{_level}", $"{_distanceToPlayer}", $"{_runTime}");
    }
}