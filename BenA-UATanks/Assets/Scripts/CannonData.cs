using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonData : MonoBehaviour
{
    public float cannonShelfLife = 1.5f;
    public float shellDamage; 
    public GameObject shooter;
    public AudioClip hit;

    

    
    void Start ()
    {
        if (shellDamage == 0)
        {
            shellDamage = 10;
        }
    }
	
	
	void Update ()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        
        TankData otherObjData = other.gameObject.GetComponent<TankData>();

        
        if (otherObjData != null)
        {
            
            otherObjData.updateHealth(shellDamage);
          //  AudioSource.PlayClipAtPoint(hit, Vector3.zero);
            
            shooter.GetComponent<TankData>().updateDamageDone(shellDamage);

            
            if (otherObjData.health <= 0)
            {
                Destroy(other.gameObject);
                GameManager.instance.highScore++;
                
            }

            
            Destroy(gameObject);
        }
        else
        {
            
        }
    }

}
