using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{   
    // collision detection between character and apples
    // will be called whenever collision happens with apple
    private void OnTriggerEnter(Collider other)
    {
        // get the player inventory component when object is collieded
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        // use the player inventory to call the apple collect 
        if (playerInventory != null)
        {
            playerInventory.AppleCollected();
            gameObject.SetActive(false); // set the apple inactive once being collected
        }
    }
}
