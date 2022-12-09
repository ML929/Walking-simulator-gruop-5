using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI AppleText;

    // Start is called before the first frame update
    void Start()
    {
        // get text component and assign to this field 
        AppleText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    // update the apple text
    public void UpdateAppleText(PlayerInventory playerInventory) // take the player inventory as the parameter
    {
        // set the text number of apples to the inventory
        AppleText.text = playerInventory.NumberOfApples.ToString();
    }
}
