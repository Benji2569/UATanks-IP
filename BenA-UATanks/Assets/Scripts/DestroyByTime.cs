using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    CannonData data;
    void Start ()
    {
        data = GetComponent<CannonData>();
        Destroy(gameObject, data.cannonShelfLife);

	}
	
	
	void Update ()
    {
		
	}

  

    
}
