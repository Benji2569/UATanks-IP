using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    List<PowerUp> powerups;
    TankData data;
    public float length;

	
	void Start ()
    {
        powerups = new List<PowerUp>();
        data = gameObject.GetComponent<TankData>();
        
}
	
	void Update ()
    {
        List<PowerUp> expired = new List<PowerUp>();
        
        foreach (var item in powerups )
        {
            
            
            item.durration -= Time.deltaTime;
            

            
            if (item.durration <= 0)
            {
                expired.Add(item);
                

            }

        }

        
        foreach (var item in expired)
        {
            item.OnDeactivate(data);
            powerups.Remove(item);
        }

        
        expired.Clear();
	}

    public void Add(PowerUp powerUp)
    {
        powerUp.OnActivate(data);

        if (!powerUp.IsPermanent())
        {
            powerups.Add(powerUp);
        }
        
    }
}
