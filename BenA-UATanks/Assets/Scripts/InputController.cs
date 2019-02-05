using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public enum Schemes
    {
        WASD,
        ARROWS
    }


    private GameManager manager;
    public Schemes input = Schemes.ARROWS;
    [HideInInspector]
    public TankMotor motor;
    [HideInInspector]
    public TankData data;
    [HideInInspector]
    public TankShoot shoot;
    [HideInInspector]
    public Transform trans;

    private float nextShoot;

    private void Awake()
    {
        
    }

    
    void Start ()
    {
        manager = GameObject.Find("Game").GetComponent<GameManager>();

        for (int i = 0; i < manager.players.Length; i++)
        {
            if (gameObject.Equals(manager.players[i]))
            {
                motor = manager.players[i].GetComponent<TankMotor>();
                data = manager.players[i].GetComponent<TankData>();
                shoot = manager.players[i].GetComponent<TankShoot>();
                trans = manager.players[i].GetComponent<Transform>();

            }
        }
        
        
        nextShoot = Time.time + data.fireRate;

    }
	
	
	void Update ()
    {
        
        switch (input)
        {
            case Schemes.WASD:
                if (Input.GetKey(KeyCode.W))
                {
                    motor.Move(data.frontSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    motor.Move(-data.backSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    motor.Turn(-data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    motor.Turn(data.turnSpeed);
                }
                break;
            case Schemes.ARROWS:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    motor.Move(data.frontSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    motor.Move(-data.backSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    motor.Turn(-data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    motor.Turn(data.turnSpeed);
                }
                break;
            default:
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            
            if (Time.time > nextShoot)
            {
                shoot.Shoot(data.shellForce);
                nextShoot = Time.time + data.fireRate;
            }


        }


    }


}
