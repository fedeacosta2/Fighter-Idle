using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawnButton : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickUp;

    public void PickupItem(int id)
    {   
        bool result = inventoryManager.AddItem(itemsToPickUp[id]);
        if (result == true)
        {
            Debug.Log("Item Added");
        }
        else
        {
            Debug.Log("inventory full");
        }
    }

    public void getSelectedItems()
    {
        Item receivedItem = inventoryManager.getSelectedItem(false);
        if (receivedItem != null)
        {
            Debug.Log("Received Item" + receivedItem);
        }
        else
        {
            Debug.Log("No Received Item" );
        }
    }
    
    public void useSelectedItems()
    {
        Item receivedItem = inventoryManager.getSelectedItem(true);
        if (receivedItem != null)
        {
            Debug.Log("Received Item" + receivedItem);
        }
        else
        {
            Debug.Log("No  Item" );
        }
    }
}
