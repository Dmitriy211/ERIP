using System.Collections.Generic;
using System.Threading.Tasks;
using RoR2;

namespace ScienceKit;

public class PlayerHealthStatistics: BaseRunStatistics<PlayerHealthEntry>
{
    private HealthComponent _playerHealth;
    private bool _enabled = true;
    private float _lastPlayerHealth;
    
    private List<PlayerHealthEntry> HealthEntries => data;

    protected override StatisticType statisticType => StatisticType.PlayerHealth;

    public PlayerHealthStatistics()
    {
        GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        HealthComponent.onCharacterHealServer += HealthComponent_onCharacterHealServer;
        StartAsyncUpdate();
    }

#pragma warning disable Publicizer001
    private HealthComponent GetPlayerHealth()
    {
        if (_playerHealth == null)
            _playerHealth = PlayerCharacterMasterController.instances[0].body?.healthComponent;
        return _playerHealth;
    }
#pragma warning restore Publicizer001

    private async void StartAsyncUpdate()
    {
        while (_enabled)
        {
            Update();
            await Task.Delay(3000);
        }
    }

    private void Update()
    {
        if (Run.instance == null) return;
        if (GetPlayerHealth() == null) return;

        if (_lastPlayerHealth != GetPlayerHealth().combinedHealth)
            CollectHealthEntry();

        _lastPlayerHealth = GetPlayerHealth().combinedHealth;
    }

    private void GlobalEventManager_onServerDamageDealt(DamageReport report)
    {
        if (report.victim != GetPlayerHealth()) return;
        CollectHealthEntry();
    }

    private void HealthComponent_onCharacterHealServer(HealthComponent victim, float amount, ProcChainMask _)
    {
        if (victim != GetPlayerHealth()) return;
        CollectHealthEntry();
    }

    private void CollectHealthEntry()
    {
        if (currentRun != Run.instance)
        {
            Deinitialize();
            Initialize();
        }
        
        if (GetPlayerHealth() == null) return;
        
        var healthEntry = new PlayerHealthEntry(
            HealthEntries.Count,
            currentRun.time,
            GetPlayerHealth().combinedHealth,
            GetPlayerHealth().combinedHealthFraction
        );
        HealthEntries.Add(healthEntry);
        Log.LogInfo($"Health entry: {healthEntry}");
    }

    public override void Dispose()
    {
        GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
        HealthComponent.onCharacterHealServer -= HealthComponent_onCharacterHealServer;
        _enabled = false;
        
        base.Dispose();
    }
}