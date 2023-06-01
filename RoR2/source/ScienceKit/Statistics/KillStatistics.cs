using System.Collections.Generic;
using RoR2;

namespace ScienceKit
{
    public class KillStatistics : BaseRunStatistics<KillEntry>
    {
        private List<KillEntry> Kills => data;

        protected override StatisticType statisticType => StatisticType.Kills;

        public KillStatistics()
        {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            if (currentRun != Run.instance)
            {
                Deinitialize();
                Initialize();
            }

            if (report.attackerMaster == null) return;
            if (report.attackerMaster != PlayerCharacterMasterController.instances[0].master) return;
            if (report.victimBody == false) return;
    
            var killEntry = new KillEntry(Kills.Count, currentRun.time, report);
            Kills.Add(killEntry);
            Log.LogInfo($"Kill entry: {killEntry}");
        }
    
    
        public override void Dispose()
        {
            GlobalEventManager.onCharacterDeathGlobal -= GlobalEventManager_onCharacterDeathGlobal;
            
            base.Dispose();
        }
    }
}