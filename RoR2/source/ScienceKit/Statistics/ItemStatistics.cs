using System.Collections.Generic;
using System.Linq;
using RoR2;
using RoR2.ContentManagement;

namespace ScienceKit;

public class ItemStatistics : BaseRunStatistics<ItemEntry>
{
    private List<ItemEntry> ItemList => data;
    
    private Inventory _playerInventory;
    private EquipmentIndex _currentEquipment = EquipmentIndex.None;
    
    protected override StatisticType statisticType => StatisticType.Items;

    public ItemStatistics()
    {
        Inventory.onServerItemGiven += Inventory_OnServerItemGiven;
        Inventory.onInventoryChangedGlobal += Inventory_OnInventoryChangedGlobal;
    }

    private void Inventory_OnServerItemGiven(Inventory inventory, ItemIndex itemIndex, int _)
    {
        if (itemIndex == ItemIndex.None) return;
        if (inventory != GetPlayerInventory()) return;

        AddItemEntry((int)itemIndex, false, true);
    }

    private void Inventory_OnInventoryChangedGlobal(Inventory inventory)
    {
        if (inventory != GetPlayerInventory()) return;
        
        if (_currentEquipment == inventory.currentEquipmentIndex) return;
        _currentEquipment = inventory.currentEquipmentIndex;
        
        AddItemEntry((int)_currentEquipment, true, true);
    }

    private void AddItemEntry(int itemIndex, bool isEquipment, bool isAdded)
    {
        if (currentRun != Run.instance)
        {
            Deinitialize();
            Initialize();
        }
        
        var itemEntry = new ItemEntry(
            ItemList.Count,
            currentRun.time,
            itemIndex,
            isEquipment,
            true
        );
        ItemList.Add(itemEntry);
        LogEntry(itemIndex, isEquipment);
    }

    private void LogEntry(int itemIndex, bool isEquipment)
    {
        if (itemIndex == -1) return;
        
        Log.LogInfo(isEquipment
            ? GetEquipmentFromIndex((EquipmentIndex)itemIndex)?.nameToken
            : GetItemFromIndex((ItemIndex)itemIndex)?.nameToken);
    }

#pragma warning disable Publicizer001
    private Inventory GetPlayerInventory()
    {
        if (_playerInventory == null)
            _playerInventory = PlayerCharacterMasterController.instances[0].body?.inventory;
        return _playerInventory;
    }
#pragma warning restore Publicizer001

    private ItemDef GetItemFromIndex(ItemIndex itemIndex)
    {
        return ContentManager.itemDefs.FirstOrDefault(i => i.itemIndex == itemIndex);
    }
    
    private EquipmentDef GetEquipmentFromIndex(EquipmentIndex equipmentIndex)
    {
        return ContentManager.equipmentDefs.FirstOrDefault(e => e.equipmentIndex == equipmentIndex);
    }
    
    public override void Dispose()
    {
        Inventory.onServerItemGiven -= Inventory_OnServerItemGiven;
        
        base.Dispose();
    }
}