using UnityEngine;
using MO.Item;
public class CheatBehaviour : MonoBehaviour {

    public int ItemId = 5;

    [ContextMenu("Equip Helmet")]
    public void EquipItem(){
        ItemObject helmet = new ItemObject(){Id = "0", Type = ItemType.Head};
        InventoryManager.Instance.Equip(helmet);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach(var player in players){
            if(player.IsLocal){
                player.EquipItems(InventoryManager.Instance.Slots);
            }
        }
    }

    [ContextMenu("Equip Iron Sword")]
    public void EquipItem2(){
        ItemObject sword = new ItemObject(){Id = "3", Type = ItemType.EquipedWeapon};
        InventoryManager.Instance.Equip(sword);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach(var player in players){
            if(player.IsLocal){
                player.EquipItems(InventoryManager.Instance.Slots);
            }
        }
    }

    [ContextMenu("Equip Bow")]
    public void EquipItem3()
    {
        ItemObject bow = new ItemObject() { Id = "4", Type = ItemType.EquipedWeapon };
        InventoryManager.Instance.Equip(bow);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach (var player in players)
        {
            if (player.IsLocal)
            {
                player.EquipItems(InventoryManager.Instance.Slots);
            }
        }
    }

    [ContextMenu("Equip Item X")]
    public void EquipItemX()
    {
        ItemObject item = new ItemObject() { Id = ItemId.ToString(), Type = ItemType.EquipedWeapon };
        InventoryManager.Instance.Equip(item);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach (var player in players)
        {
            if (player.IsLocal)
            {
                player.EquipItems(InventoryManager.Instance.Slots);
            }
        }
    }
}