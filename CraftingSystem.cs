using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public GameObject toolScreenUI;
    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button ToolsBTN;

    //Craft Buttons:
    Button craftAxeBTN;

    //Requirement TEXT:
    Text AxeReq1, AxeReq2;

    public bool isOpen;

    //All Blueprints
    private Blueprint AxeBPL = new Blueprint("Axe", 2, "Stone", 1, "Stick", 1);   

    private void Awake()
    {
        if(Instance != null && Instance != this) 
        {
            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        ToolsBTN = craftingScreenUI.transform.Find("Button_TOOLS").GetComponent<Button>();
        ToolsBTN.onClick.AddListener(delegate { OpenToolsCategory();});

        // AXE:
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBPL); });
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        //Add item to inventory
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        //Remove Resources from inventory
        if (blueprintToCraft.numOfRequeriments == 1) 
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        } 
        else if (blueprintToCraft.numOfRequeriments == 2) 
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }

        //Refresh List
        StartCoroutine(calculate());

        // RefreshNeededItems();
    }

    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.RecalculateList();
        RefreshNeededItems();
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
    }



    // Update is called once per frame
    void Update()
    {

        //RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            craftingScreenUI.SetActive(true);            
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName) 
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        // ---- AXE:
        AxeReq1.text = "- 1 Pedra [" + stone_count + "]";
        AxeReq2.text = "- 1 Graveto [" + stick_count + "]";

        if(stone_count >= 1 && stick_count >= 1 )
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }

    }


}
