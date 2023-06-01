using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;

namespace ScienceKit
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]
    public class ScienceKitPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "DanteZ";
        public const string PluginName = "ScienceKit";
        public const string PluginVersion = "0.1.0";
        
        private StatisticsPersistentManager _statisticsPersistentManager;
        private GameSimplifier _gameSimplifier;
        private readonly List<IStatistics> _statistics = new ();

        public void Awake()
        {
            Log.Init(Logger);

            _statisticsPersistentManager = new StatisticsPersistentManager();
            _gameSimplifier = new GameSimplifier();
            
            _statistics.Add(new KillStatistics());
            _statistics.Add(new SpawnStatistics());
            _statistics.Add(new ButtonsStatistics());
            _statistics.Add(new AxisStatistics());
            _statistics.Add(new PlayerHealthStatistics());
            _statistics.Add(new ItemStatistics());
            _statistics.Add(new PlayerStatsStatistics());
            
            Log.LogInfo("ScienceKit loaded");
        }

        public void OnDestroy()
        {
            foreach (var statistic in _statistics) 
                statistic.Dispose();
            
            _statisticsPersistentManager.Dispose();
            _gameSimplifier.Dispose();
        }
    }
}
