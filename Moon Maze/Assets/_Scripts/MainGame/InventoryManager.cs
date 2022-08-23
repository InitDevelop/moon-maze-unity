using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public Sprite SelectedSlotSprite;
    public Sprite NonSelectedSlotSprite;

    public GameObject FlashLight;

    public Image[] SlotPanels;
    public Image[] ItemImages;

    public Sprite[] ItemSprites;

    public byte CurrentSlotNumber = 0;      // Possible values are 0, 1, 2

    // Start is called before the first frame update
    void Start()
    {
        CurrentSlotNumber = 0;
        SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = SelectedSlotSprite;

        ItemImages[0].GetComponent<Image>().sprite = ItemSprites[0];
        ItemImages[1].GetComponent<Image>().enabled = false;
        ItemImages[2].GetComponent<Image>().enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = NonSelectedSlotSprite;
            CurrentSlotNumber = 0;
            SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = SelectedSlotSprite;
            FlashLight.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = NonSelectedSlotSprite;
            CurrentSlotNumber = 1;
            SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = SelectedSlotSprite;
            FlashLight.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = NonSelectedSlotSprite;
            CurrentSlotNumber = 2;
            SlotPanels[CurrentSlotNumber].GetComponent<Image>().sprite = SelectedSlotSprite;
            FlashLight.SetActive(false);
        }
    }
}
