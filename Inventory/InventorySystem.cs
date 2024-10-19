using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{


    public static InventorySystem Instance { get; set; }


    public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<String> itemList = new List<String>();
    private GameObject itemToAdd;
    private GameObject itemToSlotEquip;

    public bool isOpen;
    public int itemQtdInvFull;
    //public bool isFull;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;

        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform) 
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)
        {

            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddToInventory(String itemName)
    { 
        
        itemToSlotEquip = FindNextEmptySlot();
        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), itemToSlotEquip.transform.position, itemToSlotEquip.transform.rotation);
        itemToAdd.transform.SetParent(itemToSlotEquip.transform);
        itemList.Add(itemName);

        RecalculateList();
        CraftingSystem.Instance.RefreshNeededItems();


    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
            foreach (GameObject slot in slotList) 
            {
                if(slot.transform.childCount > 0)
                {
                    counter += 1;
                }
            
            }
        if (counter == itemQtdInvFull)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for (var i = slotList.Count - 1; i >=0; i--)
        {
            if(slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }

        RecalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
    
    public void RecalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str1 = name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }



}