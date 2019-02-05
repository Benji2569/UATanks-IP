using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public PowerUp modData;
    public AudioClip feedback;
    
	void Start () {
        
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        
        
        PowerUpController puController = other.gameObject.GetComponent<PowerUpController>();
        Transform tran = other.gameObject.GetComponent<Transform>();

        
        if (puController != null) {
            puController.Add(modData);

            
            if (feedback != null) {
                AudioSource.PlayClipAtPoint(feedback, Vector3.zero, 5.0f);
            }

            
            Destroy(gameObject);
        }
    }
}
