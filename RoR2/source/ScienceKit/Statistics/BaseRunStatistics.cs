using System.Collections.Generic;
using System.Linq;
using RoR2;

namespace ScienceKit
{
    public abstract class BaseRunStatistics<T>: IStatistics where T : IEntry
    {
        private bool _initialized;

        protected readonly List<T> data = new ();
        protected Run currentRun;

        protected abstract StatisticType statisticType { get; }

        protected void Initialize()
        {
            if (_initialized) return;
            if (Run.instance == null) return;
            
            currentRun = Run.instance;
            StatisticsPersistentManager.instance.PrepareFile(statisticType);
            data.Clear();
            
            _initialized = true;
        }
    
        protected void Deinitialize()
        {
            if (_initialized == false) return;
            
            SaveStatistics();
            
            _initialized = false;
        }
    
        protected void SaveStatistics()
        {
            if (data.Count == 0) return;

            var stringEntries = new []{ data[0].Columns };
            StatisticsPersistentManager.instance.WriteStatisticsEntries(
                statisticType,
                stringEntries.Concat(data.Select(k => k.ToString()))
            );
        }
        
        public virtual void Dispose()
        {
            Deinitialize();
        }
    }
}