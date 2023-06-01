using System;
using System.Collections.Generic;
using System.IO;
using RoR2;
using UnityEngine;

namespace ScienceKit
{
    public class StatisticsPersistentManager: IDisposable
    {
        public static StatisticsPersistentManager instance => _instance;

        private static StatisticsPersistentManager _instance;
        private readonly Dictionary<StatisticType, string> _statisticsPaths = new();

        public StatisticsPersistentManager()
        {
            _instance = this;
        }
    
        public void PrepareFile(StatisticType type)
        {
            string statisticsPath = $"{Application.persistentDataPath}/Statistics/{type.ToString()}";
            string currentRunStartTime = Run.instance.GetStartTimeUtc().ToString("yy-MM-dd-hh-mm-ss");
            string filePath = statisticsPath + $"/{type.ToString()}-{currentRunStartTime}.csv";
            
            if (File.Exists(filePath) == false)
            {
                Directory.CreateDirectory(statisticsPath);
                File.Create(filePath);
                Log.LogInfo($"created file {filePath}");
            }
            
            _statisticsPaths[type] = filePath;
        }

        public void WriteStatisticsEntries(StatisticType type, IEnumerable<string> entries)
        {
            using StreamWriter killsStreamWriter = new StreamWriter(_statisticsPaths[type], true);
            foreach (var entry in entries)
            {
                killsStreamWriter.WriteLine(entry);
            }
        }

        public void Dispose()
        {
        }
    }
}