using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   public static InventoryManager instance;
   [SerializeField] private PlayerController player;
   [SerializeField] private GameObject boatSpawn;
   [SerializeField] private GameObject hammer;
   
   private Animator playerAnimator;
   public Item[] startItems;
   public InventorySlot[] invetorySlots;
   public GameObject inventoryItemPrefab;
   public int maxStackedItems = 4;
   private int selectedSlot = -1;
   private Camera cam;
   //public bool dragActive = false;
   //public static bool dragActive = false;
   
   public Dictionary<string, GameObject> itemObjectMap = new Dictionary<string, GameObject>();
   //Items GameObjects
   public GameObject swordObject;
   public GameObject logObject;
   public GameObject axeObject;
   public GameObject rockObject;
   public GameObject flowerObject;
   public GameObject mushroomObject;
   public GameObject branchObject;
   public GameObject plantObject;
   private void Awake()
   {
      instance = this;
   }

   private void Start()
   {
      playerAnimator = player.GetComponentInChildren<Animator>();
      // Initialize itemObjectMap with mappings from item names to corresponding GameObjects
      InitializeItemObjectMap();
      Debug.Log(invetorySlots.Length);
      
      
      
      foreach (var item in startItems)
      {
         AddItem(item);
      }
   }

   
   public void DeactivateCurrentItemInSlot()
   {
      if (selectedSlot >= 0 && selectedSlot < invetorySlots.Length)
      {
         InventorySlot currentSlot = invetorySlots[selectedSlot];
         InventoryItem currentItemInSlot = currentSlot.GetComponentInChildren<InventoryItem>();
         if (currentItemInSlot != null)
         {
            // Deactivate the corresponding GameObject of the currently selected item
            DeactivateCorrespondingObject(currentItemInSlot.item);
            currentSlot.DeSelect();
         }
      }
   }

   void ChangeSelectedSlot(int newValue)
   {
      if (selectedSlot >= 0)
      {
         InventorySlot previousSlot = invetorySlots[selectedSlot];
         InventoryItem previousItemInSlot = previousSlot.GetComponentInChildren<InventoryItem>();
         if (previousItemInSlot != null)
         {
            // Deactivate the corresponding GameObject of the previously selected item
            DeactivateCorrespondingObject(previousItemInSlot.item);
         }
         previousSlot.DeSelect();
      }

      invetorySlots[newValue].Select();
      selectedSlot = newValue;
   }

   void DeactivateCorrespondingObject(Item item)
   {
      if (itemObjectMap.ContainsKey(item.name))
      {
         GameObject correspondingObject = itemObjectMap[item.name];
         if (correspondingObject != null)
         {
            correspondingObject.SetActive(false);
         }
      }
   }
   
   void InitializeItemObjectMap()
   {
      // Add mappings from item names to corresponding GameObjects
      // Replace "SwordObject", "LogObject", "AxeObject" with actual references to GameObjects in your scene
      itemObjectMap.Add("Sword", swordObject);
      itemObjectMap.Add("Log", logObject);
      itemObjectMap.Add("Axe", axeObject);
      itemObjectMap.Add("Rock", rockObject);
      itemObjectMap.Add("Flower", flowerObject);
      itemObjectMap.Add("Mushroom", mushroomObject);
      itemObjectMap.Add("Branch", branchObject);
      itemObjectMap.Add("Plant", plantObject);
   }
   
   
   public bool AddItem(Item item)
   {
      
      //Check if any slot has the same item with count lower than max and stack them
      for (int i = 0; i<invetorySlots.Length; i++)
      {
         InventorySlot slot = invetorySlots[i];
         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
         if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems )
         {
            itemInSlot.count++;
            itemInSlot.RefreshCount();
            return true;
         }
      }
      
      //Find an Empty Slot
      for (int i = 0; i<invetorySlots.Length; i++)
      {
         InventorySlot slot = invetorySlots[i];
         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
         if (itemInSlot == null)
         {
            SpawnNewItem(item, slot);
            return true;
         }
      }
      Debug.Log("no hay espacio");
      return false;
      

   }

   

   public int GetItemCount(ItemType itemType)
   {
      int count = 0;
      foreach (var slot in invetorySlots)
      {
         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
         if (itemInSlot != null && itemInSlot.item.type == itemType)
         {
            count += itemInSlot.count;
         }
      }
      return count;
   }
   
   // Method to activate a specific GameObject based on item counts
   public void ActivateGameObjectBasedOnItemCount()
   {
      int logCount = GetItemCount(ItemType.Log);
      int rockCount = GetItemCount(ItemType.Rock);
      
      

      
      if (logCount >= 4 && rockCount >= 1)
      {
         
         StartCoroutine(SpawnTheBoatWithDelay(2.5f));
         // Translate and rotate the character
         Vector3 newPosition = new Vector3(-5.807014f, 0.3032467f, 34.58574f);
         Quaternion newRotation = Quaternion.Euler(0f, 293.84f, 0.001f);
         player.transform.position = newPosition;
         player.transform.rotation = newRotation;
         // Decrease item count or destroy the item in the slot
         foreach (var slot in invetorySlots)
         {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && (itemInSlot.item.type == ItemType.Log || itemInSlot.item.type == ItemType.Rock))
            {
               if (itemInSlot.count > 1)
               {
                  itemInSlot.count-=4;
                  itemInSlot.RefreshCount();
               }
               else
               {
                  Destroy(itemInSlot.gameObject);
               }
            }
         }
      }
      
   }

   private IEnumerator SpawnTheBoatWithDelay(float delay)
   {
      // Start the Axe_Slash animation
      playerAnimator.Play("Axe_Slash");
      InventorySlot slot = invetorySlots[selectedSlot];
      DeactivateCorrespondingObject(slot.GetComponentInChildren<InventoryItem>().item);
      hammer.SetActive(true);

      // Wait for the animation to finish
      yield return new WaitForSeconds(delay);

      // Deactivate the boat after the delay
      boatSpawn.SetActive(true);
      hammer.SetActive(false);

      // Loop until the coroutine is finished
      while (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Axe_Slash"))
      {
         // Wait for the animation to finish before playing it again
         yield return null;
      }

      // Coroutine finished, do any cleanup here
   }
   
   void SpawnNewItem(Item item, InventorySlot slot)
   {
      GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
      InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
      inventoryItem.InitialiseItem(item);
   }

   public void ChangeToSlot0()
   {
      ChangeSelectedSlot(0);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
      
      
   }
   
   public void ChangeToSlot1()
   {
      ChangeSelectedSlot(1);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
   }
   
   public void ChangeToSlot2()
   {
      ChangeSelectedSlot(2);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
   }
   
   public void ChangeToSlot3()
   {
      ChangeSelectedSlot(3);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
   }
   
   public void ChangeToSlot4()
   {
      ChangeSelectedSlot(4);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
   }
   
   public void ChangeToSlot5()
   {
      ChangeSelectedSlot(5);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
   }
   
   public void ChangeToSlot6()
   {
      ChangeSelectedSlot(6);
      getSelectedItem(false);
      playerAnimator.Play("Equip_Weapon");
   }

   public Item getSelectedItem(bool use)
   {
      InventorySlot slot = invetorySlots[selectedSlot];
      InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
      if (itemInSlot != null)
      {
         Item item = itemInSlot.item;
         if (use)
         {
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
               Destroy(itemInSlot.gameObject);
            }
            else
            {
               itemInSlot.RefreshCount();
            }
         }
         else // If not using the item
         {
            // Activate the corresponding GameObject in the scene based on the selected item
            if (itemObjectMap.ContainsKey(item.name))
            {
               GameObject correspondingObject = itemObjectMap[item.name];
               if (correspondingObject != null)
               {
                  StartCoroutine(ActivateObjectWithDelay(correspondingObject, 0.2f));
               }
               
            }
         }

         return item;
      }

      return null;
   }
   
   private IEnumerator ActivateObjectWithDelay(GameObject obj, float delay)
   {
      yield return new WaitForSeconds(delay);
      obj.SetActive(true);
   }
   
   
}
