using System.Collections;
using System.Collections.Generic;
using MO.Utils;
using UnityEngine;

namespace MO.Item
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemObject[] Slots = new ItemObject[512];

        public const int MaxEquipedSlotIdx = 63;

        public void Equip(ItemObject item)
        {
            MoveItemFromSlotToInventory(item.Type);
            Slots[(int)item.Type] = item;
        }

        public void MoveItemFromSlotToInventory(ItemType type){
            int idx = (int)type;
            if(Slots[idx] != null){
                var item = Slots[idx];
                Slots[idx] = null;
                AddItemToInventory(item);
            }
        }

        public void AddItemToInventory(ItemObject item){
            int newIdx = FindInventoryFirstFreeSlot();
            if(newIdx >= 0)
                Slots[newIdx] = item;
            else
                throw new System.Exception("Inventory is full, item lost...");
        }

        public int FindInventoryFirstFreeSlot(){
            for (int i = MaxEquipedSlotIdx+1; i < Slots.Length; i++)
            {
                if(Slots[i] == null)
                    return i;
            }
            return -1; // We should hanble the full inventory ...
        }

    }

    public class ItemObject
    {
        public string Id;
        public ItemType Type;
    }

    public enum ItemType{
        Head = 0,
        Shoulder = 1,
        Chest = 2,
        Belt = 3,
        Legs = 4,
        Feet = 5,
        EquipedWeapon = 16
    }
}

