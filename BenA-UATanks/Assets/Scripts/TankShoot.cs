using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [HideInInspector]
    public GameObject cannonball;
    public GameObject prefabCannon;
    public Transform cannonSpawn;
    [HideInInspector]
    public Transform trans;
    CannonData data;
    private TankData tdata;
    

    
    

    private void Awake()
    {
        trans = gameObject.GetComponent<Transform>();
        
    }


    
    void Start()
    {
        
        prefabCannon = GameManager.instance.prefabCannon;

        tdata = gameObject.GetComponent<TankData>();
    }

    

    public void Shoot(float force)
    {
        
        cannonball = Instantiate(prefabCannon, trans.position + trans.forward, trans.rotation);
        
        data = cannonball.GetComponent<CannonData>();
        data.shooter = trans.gameObject;
         
        if (data.shooter.CompareTag("Player"))
        {
            data.shellDamage += tdata.damageMod;
        }
        else
        {
            data.shellDamage = 8;
        }

        
        cannonball.GetComponent<Rigidbody>().AddForce(force * trans.forward);

    }
}
