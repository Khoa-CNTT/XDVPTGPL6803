using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    /*
    [SerializeField]
    Inventory inventory;
    public Transform inventoryContainer;
    public Transform itemSlotTemplate;
    private RectTransform[] UIItemSlots;
    private Button[] buttons;
    private Slot pickedItem = new Slot();
    private RectTransform pickedItemSlot;
    private void Start()
    {
        //itemSlotTemplate = inventoryContainer.Find("containerTemplate");
        pickedItemSlot = Instantiate(itemSlotTemplate, this.transform).GetComponent<RectTransform>();
    }

    void pickItem(int index) 
    {
        pickedItem = inventory.GetInventory()[index].Copy();
        inventory.GetInventory()[index].Clear();
        UIItemSlots[index].GetChild(0).Find("Icon").gameObject.SetActive(false);
    }
    void placeItem() 
    {
    
    }
    public void buttonEvent(int index) 
    {
        if (pickedItem.GetItem() == null && inventory.GetInventory()[index].GetItem() != null)
        {
            pickItem(index);
        }
        else
        {
            if (pickedItem.GetItem() != null) 
            {
                int rest = inventory.Add(pickedItem.GetItem(), pickedItem.GetCount(), index);
                if (rest == 0)
                {
                    pickedItem.Clear();
                }
                else
                {
                    pickedItem.Count = rest;
                }
                //placeItem(index);
            }

        }
        
        //Debug.Log("button " + index + " has been pressed!");
    }
    public void setInventory(Inventory inventory) 
    {
        inventory.onItemChangedCallback += updateInventory;
        updateInventory();
    }
    private void Update()
    {
        //buttons[index] = itemSlotRectTransform.Find("Button").GetComponent<Button>();
        //itemSlotRectTransform.Find("Button").GetComponent<Identity>().index = index;
        //buttons[index].onClick.AddListener(() => buttonEvent(itemSlotRectTransform.Find("Button").GetComponent<Identity>().index));
        pickedItemSlot.anchoredPosition = new Vector2(Input.mousePosition.x, 0);
        pickedItemSlot.gameObject.SetActive(true);
        pickedItemSlot.gameObject.name = "pickedItem";

        if (pickedItem.ItemType != null)
        {
            Transform item = pickedItemSlot.GetChild(0).Find("Icon");
            item.gameObject.SetActive(true);
            item.GetComponent<Image>().sprite = pickedItem.ItemType.sprite;
        }
        else 
        {
            Transform item = pickedItemSlot.GetChild(0).Find("Icon");
            item.gameObject.SetActive(false);
        }
    }
    private void updateInventory()
    {
        
        int rows = 2;
        int columns = 4;
        float slotSize = 95;
        int x = 0;
        int y = 0;
        int index = 0;
        if (UIItemSlots != null) {
            foreach (RectTransform ItemSLot in UIItemSlots)
            {
                Destroy(ItemSLot.gameObject);
            }
        }
        
        UIItemSlots = new RectTransform[inventory.Count];
        buttons = new Button[inventory.Count];
        //inventoryContainer.GetComponent<RectTransform>().
        foreach (Slot slot in inventory) 
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, inventoryContainer).GetComponent<RectTransform>();
            buttons[index] = itemSlotRectTransform.Find("Button").GetComponent<Button>();
            itemSlotRectTransform.Find("Button").GetComponent<Identity>().index = index;
            buttons[index].onClick.AddListener(() => buttonEvent(itemSlotRectTransform.Find("Button").GetComponent<Identity>().index));
            itemSlotRectTransform.anchoredPosition = new Vector2(x* slotSize + slotSize * 0.5f,y * slotSize - slotSize * 0.5f );
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.gameObject.name = "ItemSlot" + index;

            if (slot.GetItem() != null) 
            {
                Transform item = itemSlotRectTransform.GetChild(0).Find("Icon");
                Transform itemCount = itemSlotRectTransform.GetChild(0).Find("Count");
                item.gameObject.SetActive(true);
                item.GetComponent<Image>().sprite = slot.GetItem().sprite;
                if (slot.GetCount() > 1) 
                {
                    itemCount.gameObject.SetActive(true);
                    itemCount.GetComponent<TextMeshProUGUI>().text = slot.GetCount().ToString();
                }
            }
            x++;
            if (x > 3) 
            {
                x = 0;
                y--;
            }
            UIItemSlots[index] = itemSlotRectTransform;
            index++;
        }
    }*/
}
