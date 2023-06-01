using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoR2;

namespace ScienceKit;

public class PlayerStatsStatistics: BaseRunStatistics<PlayerStatsEntry>
{
    private List<PlayerStatsEntry> StatsEntries => data;
    
    private CharacterBody _playerBody;
    private bool _enabled = true;

    private List<float> _speedHistory = new();
    private List<float> _damageHistory = new();

    protected override StatisticType statisticType => StatisticType.Stats;

    public PlayerStatsStatistics()
    {
        Run.onRunStartGlobal += StartStatistics;
        GlobalEventManager.onServerDamageDealt += OnDamageDealt;
    }

#pragma warning disable Publicizer001
    private CharacterBody GetPlayerBody()
    {
        if (_playerBody == null)
            _playerBody = PlayerCharacterMasterController.instances[0].body;
        return _playerBody;
    }
#pragma warning restore Publicizer001

    private void OnDamageDealt(DamageReport report)
    {
        if (report.attackerBody != _playerBody) return;
        _damageHistory.Add(report.damageDealt);
    }

    private void StartStatistics(Run run)
    {
        StartFrameUpdate();
        StartSecondUpdate();
    }

    private async void StartFrameUpdate()
    {
        while (_enabled)
        {
            Update();
            await Task.Yield();
        }
    }

    private async void StartSecondUpdate()
    {
        while (_enabled)
        {
            AppendStats();
            await Task.Delay(1000);
        }
    }

    private void Update()
    {
        if (Run.instance == null) return;
        var playerBody = GetPlayerBody();
        if (playerBody == null) return;
        
        _speedHistory.Add(playerBody.rigidbody.velocity.magnitude);
    }

    private void AppendStats()
    {
        if (Run.instance == null) return;
        
        if (currentRun != Run.instance)
        {
            Deinitialize();
            Initialize();
        }

        var averageSpeed = _speedHistory.Count == 0 ? 0 : _speedHistory.Average();
        var dps = _damageHistory.Sum();
        _speedHistory.Clear();
        _damageHistory.Clear();
        
        var statEntry = new PlayerStatsEntry(
            StatsEntries.Count,
            Run.instance.time,
            averageSpeed,
            dps
        );
        StatsEntries.Add(statEntry);
        
        Log.LogInfo($"Stats entry: {statEntry}");
    }

    public override void Dispose()
    {
        Run.onRunStartGlobal -= StartStatistics;
        _enabled = false;
        
        base.Dispose();
    }
}