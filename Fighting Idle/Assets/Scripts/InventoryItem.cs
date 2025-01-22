using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Item item;
    [Header("UI")] public Image image;
    public TextMeshProUGUI CounText;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int count = 1;
    //[SerializeField] private InventoryManager _inventoryManager;
    public static bool dragActive = false;
    public bool OnEndDragItem = false;
    

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        count = 1;
        RefreshCount();
    }
    
    
    public void RefreshCount()
    {
        CounText.text = count.ToString();
        bool textActive = count > 1;
        CounText.gameObject.SetActive(textActive);
    }

    public void ToggleDrag() //this function is called in the button that opens the inventory, allows to drag and swap items in the inventory and toolbar
    {
        dragActive = true;
    }
    
    public void ToggleDragOff() //this function is called in the "background" button of main inventory, after dragactive = false you cant no longer drag or swap items but you can change the slot of the toolbar (be able to change weapons basically)
    {
        dragActive = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!dragActive)
            return;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragActive)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragActive)
            return;

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        OnEndDragItem = true;
    }
}
