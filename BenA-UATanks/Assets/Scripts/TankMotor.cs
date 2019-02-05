using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TankData))]
public class TankMotor : MonoBehaviour
{
    
    private CharacterController character;
    

   
    public Transform trans; 

    
    private void Awake()
    {
        if (trans == null)
        {
            
            trans = gameObject.GetComponent<Transform>();
        }
        
    }

    
    void Start ()
    {
        
        character = gameObject.GetComponent<CharacterController>();

        
            
    }
	
	
	void Update ()
    {
        
    }

   
    public void Move(float speed)
    {
        
        Vector3 forwardSpeed = trans.forward * speed;
              

        
        character.SimpleMove(forwardSpeed);
    }

    
    public void Turn(float speed)
    {
        
        Vector3 rotationSpeed = Vector3.up * speed * Time.deltaTime;

        
        trans.Rotate(rotationSpeed, Space.Self);
    }

    public bool TurnTowards(Vector3 destination, float rotateSpeed)
    {

        
        Vector3 diffToDestination = destination - trans.position;

        
        Quaternion lookat = Quaternion.LookRotation(diffToDestination);

        if (trans.rotation != lookat)
        {
            
            float maxRotate = rotateSpeed * Time.deltaTime;

            
            trans.rotation = Quaternion.RotateTowards(trans.rotation, lookat, maxRotate);


            return true;
        }
        else
        {
            return false;
        }


    }
}
