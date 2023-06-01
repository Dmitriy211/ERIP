using RoR2;
using UnityEngine;

namespace ScienceKit;

public readonly struct SpawnEntry: IEntry
{
    private readonly int _index;
    private readonly int _entityID;
    private readonly string _entityName;
    private readonly float _level;
    private readonly float _distanceToPlayer;
    private readonly float _spawnedDirection;
    private readonly float _runTime;
    
    private static string Columns => RowFormatter.FormatRow("", "EntityID", "EntityName", "Level", "DistanceToPlayer", "SpawnedDirection", "RunTime");
    string IEntry.Columns => Columns;

#pragma warning disable Publicizer001
    public SpawnEntry(int index, float runTime, CharacterBody spawnedBody)
    {
        _index = index;
        _entityID = spawnedBody.GetHashCode();
        _entityName = spawnedBody.baseNameToken;
        _level = spawnedBody.level;
        
        var playerTransform = PlayerCharacterMasterController.instances[0].body?.coreTransform;
        if (playerTransform == null)
        {
            // Invalid values if player is not yet spawned
            _distanceToPlayer = -1;
            _spawnedDirection = -2;
        }
        else
        {
            _distanceToPlayer = Vector3.Distance(spawnedBody.corePosition, playerTransform.position);
            _spawnedDirection = Vector3.Dot(
                playerTransform.forward, 
                (spawnedBody.corePosition - playerTransform.position).normalized
            );
        }
        
        _runTime = runTime;
    }
#pragma warning restore Publicizer001

    public override string ToString()
    {
        return RowFormatter.FormatRow(
            $"{_index}", 
            $"{_entityID}",
            _entityName, 
            $"{_level}", 
            $"{_distanceToPlayer}", 
            $"{_spawnedDirection}", 
            $"{_runTime}"
        );
    }
}