using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] puPrefab;
    public float timeToSpawn;
    private float nextTimeToSpawn;
    private Transform trans;
    private GameObject pickedup;
    private GameManager puManager;

	
	void Start ()
    {
        
        trans = gameObject.GetComponent<Transform>();
        nextTimeToSpawn = Time.time + timeToSpawn;

        puPrefab = Resources.LoadAll<GameObject>("PowerUps");

        if (timeToSpawn <= 0.0)
        {
            timeToSpawn = 5.0f;
        }


	}
	
	
	void Update ()
    {
        
        if (pickedup == null)
        {
            
            if (Time.time > nextTimeToSpawn)
            {
                int index = Random.Range(0, puPrefab.Length);

                pickedup = Instantiate(puPrefab[index], trans.position, Quaternion.identity) as GameObject;
                nextTimeToSpawn = Time.time + timeToSpawn;
            }
        }
        else
        {
            nextTimeToSpawn = Time.time + timeToSpawn;
        }
	}
}
