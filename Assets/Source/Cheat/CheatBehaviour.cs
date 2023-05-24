using UnityEngine;
using MO.Item;
public class CheatBehaviour : MonoBehaviour {
    
    [ContextMenu("Equip Helmet")]
    public void EquipItem(){
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        ItemObject helmet = new ItemObject(){Id = "0", Type = ItemType.Helmet};
        inventoryManager.Equip(helmet);
        var players = FindObjectsOfType<CharacterBehavior>();
        foreach(var player in players){
            if(player.IsLocal){
                player.Appeareance.ApplyEquipedItems(inventoryManager.Slots);
            }
        }
    }
}