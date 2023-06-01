using System.Collections.Generic;
using RoR2;

namespace ScienceKit
{
    public class SpawnStatistics : BaseRunStatistics<SpawnEntry>
    {
        private List<SpawnEntry> Spawns => data;

        protected override StatisticType statisticType => StatisticType.Spawns;

        public SpawnStatistics()
        {
            SpawnCard.onSpawnedServerGlobal += OnSpawnedServerGlobal;
        }

        private void OnSpawnedServerGlobal(SpawnCard.SpawnResult spawnResult)
        {
            if (spawnResult.success == false) return;
    
            if (currentRun != Run.instance)
            {
                Deinitialize();
                Initialize();
            }
            
            if (spawnResult.spawnedInstance.TryGetComponent(out CharacterMaster master))
            {
                var spawnEntry = new SpawnEntry(Spawns.Count, currentRun.time, master.GetBody());
                Spawns.Add(spawnEntry);
                Log.LogInfo($"Spawn entry: {spawnEntry}");
            }
        }
    
    
        public override void Dispose()
        {
            SpawnCard.onSpawnedServerGlobal -= OnSpawnedServerGlobal;
            
            base.Dispose();
        }
    }
}