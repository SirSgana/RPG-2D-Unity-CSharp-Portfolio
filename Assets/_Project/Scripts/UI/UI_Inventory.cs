using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private UI_ItemSlot[] uiItemSlot;
    private UI_EquipSlot[] uiEquipSlot;
    private Inventory_Player inventory;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiItemSlot = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipSlot = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindAnyObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;

        for (int i = 0; i < uiEquipSlot.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if (playerEquipSlot.HasItem() == false)
                uiEquipSlot[i].UpdateSlot(null);
            else
                uiEquipSlot[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for (int i = 0; i < uiItemSlot.Length; i++)
        {
            if (i < itemList.Count)
            {
                uiItemSlot[i].UpdateSlot(itemList[i]);
            }
            else
            {
                uiItemSlot[i].UpdateSlot(null);
            }
        }
    }
}
