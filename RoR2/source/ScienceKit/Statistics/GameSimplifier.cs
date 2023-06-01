using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using DirectorCore = On.RoR2.DirectorCore;

namespace ScienceKit;

public class GameSimplifier: IDisposable
{
    private List<string> _bannedItems = new()
    {
        "iscScrapper",
        "iscDuplicator",
        "iscDuplicatorLarge",
        "iscDuplicatorMilitary",
        "iscDuplicatorWild",
        "iscLunarChest",
        "iscGoldshoresPortal",
        "iscShopPortal",
        "iscRadarTower",
        "iscShrineBlood",
        "iscShrineBloodSandy",
        "iscShrineBloodSnowy",
        "iscShrineBoss",
        "iscShrineBossSandy",
        "iscShrineBossSnowy",
        "iscShrineChance",
        "iscShrineChanceSandy",
        "iscShrineChanceSnowy",
        "iscShrineCleanse",
        "iscShrineCleanseSandy",
        "iscShrineCleanseSnowy",
        "iscShrineCombat",
        "iscShrineCombatSandy",
        "iscShrineCombatSnowy",
        "iscShrineGoldshoresAccess",
        "iscShrineHealing",
        "iscShrineRestack",
        "iscShrineRestackSandy",
        "iscShrineRestackSnowy",
        "iscLunarTeleporter",
        "iscGoldshoresBeacon",
        "iscVoidRaidSafeWard",
        "iscDeepVoidPortal",
        "iscDeepVoidPortalBattery",
        "iscVoidPortal",
        "iscVoidCamp",
        "iscBrokenDrone1",
        "iscBrokenDrone2",
        "iscBrokenEmergencyDrone",
        "iscBrokenEquipmentDrone",
        "iscBrokenFlameDrone",
        "iscBrokenMegaDrone",
        "iscBrokenMissileDrone",
        "iscBrokenTurret1"
    };
    
    public GameSimplifier()
    {
        DirectorCore.TrySpawnObject += DirectorCoreOnTrySpawnObject;
    }

    private GameObject DirectorCoreOnTrySpawnObject(DirectorCore.orig_TrySpawnObject orig, RoR2.DirectorCore self, DirectorSpawnRequest directorSpawnRequest)
    {
        if (_bannedItems.Contains(directorSpawnRequest.spawnCard.name))
        {
            Log.LogWarning($"{directorSpawnRequest.spawnCard.name} avoided");
            return null;
        }

        return orig(self, directorSpawnRequest);
    }

    public void Dispose()
    {
        DirectorCore.TrySpawnObject -= DirectorCoreOnTrySpawnObject;
    }
}