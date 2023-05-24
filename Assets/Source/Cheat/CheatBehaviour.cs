using UnityEngine;
using MO.Item;
public class CheatBehaviour : MonoBehaviour {
    
    [ContextMenu("Equip Helmet")]
    public void EquipItem(){
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        ItemObject helmet = new ItemObject(){Id = "0", Type = ItemType.Head};
        inventoryManager.Equip(helmet);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach(var player in players){
            if(player.IsLocal){
                player.Appeareance.ApplyEquipedItems(inventoryManager.Slots);
            }
        }
    }

    [ContextMenu("Equip Iron Sword")]
    public void EquipItem2(){
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        ItemObject helmet = new ItemObject(){Id = "3", Type = ItemType.EquipedWeapon};
        inventoryManager.Equip(helmet);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach(var player in players){
            if(player.IsLocal){
                player.Appeareance.ApplyEquipedItems(inventoryManager.Slots);
            }
        }
    }
}