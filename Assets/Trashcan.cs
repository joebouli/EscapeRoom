using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : MonoBehaviour {
    private bool enabled; // A boolean flag to control whether the Trashcan is enabled or not.
    public GameObject game; // A reference to the game object.
    
    public void OnTriggerEnter(Collider other){
        if(!enabled) return;

        if(other.CompareTag("ToTrash")){
            Destroy(other.gameObject); // Destroy the object that entered the trigger zone.
            game.GetComponent<Player>().ThrowToTrash(); // Call a method on the Player component.
            gameObject.GetComponent<AudioSource>().Play(); // Play an audio source attached to this object.
        }
    }

    public void Enable(){ enabled = true; } // Public method to enable the Trashcan.
    public void Disable(){ enabled = false; } // Public method to disable the Trashcan.
}
