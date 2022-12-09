using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // trigger this when apple is collected 

public class PlayerInventory : MonoBehaviour
{
    // create new interger property for the number of apples collected
    public int NumberOfApples { get; private set;} // any other setter can read the value only this can set the value

    // 
    public UnityEvent<PlayerInventory> OnAppleCollected; //adding a unity spector

    public void AppleCollected()
    {
        NumberOfApples++;   // implement number of apple collected
        OnAppleCollected.Invoke(this); // apple collected will invoke this event
    }
}
