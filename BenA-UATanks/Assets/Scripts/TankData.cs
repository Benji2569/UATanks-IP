using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    
    public float frontSpeed; 
    public float backSpeed;
    public float turnSpeed; 
    public float shellForce; 
    public float damageDone; 
    public float fireRate; 
    public float maxHealth;
    public int score = 0; 
    public float damageMod; 
    public int lives;
    public float health; 
    

    
    void Start ()
    {
        health = maxHealth;

        if (damageMod == 0)
        {
            
            damageMod = Random.Range(1, 10);
        }
	}

    
    public void updateHealth(float shellDamage)
    {
        if (health > 0)
        {
            health -= shellDamage;
        }
}

    
    public void updateDamageDone(float damage)
    {
        damageDone += damage;
    }

    public float CheckHealth()
    {
        return health;
    }

    private void OnTriggerEnter(Collider other)
    {
        SphereCollider shield = gameObject.GetComponent<SphereCollider>();

        if (shield != null)
        {
            Destroy(shield);
        }
    }


}
