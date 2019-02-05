using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public enum EnemyMode { PATROL, CHASE, RUN, SHOOT, REST, INVESTIGATE, AVOIDING }
    public enum Behavior { AGGRESSIVE, NERVOUS, TIMID }

    [Header("Main Variables")]
    private GameManager aiManager;
    public TankMotor aiMotor;
    public TankData aiData;
    public TankShoot aiShoot;
    public Transform aiTrans;
    public EnemyMode aiMode;
    public Behavior aiBehavior;

    [Header("Shooting info")]
    private float lastShot;
    public float maxDistanceToShoot = 15.0f;

    [Header("Avoiding info")]
    public bool avoid; 
    private int avoidingStep; 
    public EnemyMode defaultMode;
    public float angle = 50.0f; 
    public float sensorLength = 1.0f;

    [Header("Sensor info")]
    private Vector3 start; 
    private Vector3 rtEndAngle; 
    private Vector3 lftEndAngle; 
    private RaycastHit hitInfo; 
    
    private Ray front;
    private Ray rtFront;
    private Ray lftFront;

    [Header("Chasing & Running info")]
    Vector3 chaseRunTarget; 
    float distanceToTarget; 
    private int targetIndex; 
    public float timeToChaseRun; 
    private float chaseRunTime; 

    [Header("Patol Info")]
    public GameObject[] patrolPath;
    [SerializeField] private GameObject patrol;
    private bool isForward;
    private int destination;
    public float close;
    public enum Pathway { Loop, PingPong };
    public Pathway pathway;

    [Header("FOV info")]
    public float fieldOfView = .45f;
    public float viewDistance = 15f;
    public float hearDistance = 15f;

    
    void Start()
    {
        
        aiManager = GameObject.Find("Game").GetComponent<GameManager>();

        
        for (int i = 0; i < aiManager.enemies.Length; i++)
        {
            if (gameObject.Equals(aiManager.enemies[i]))
            {
                aiMotor = aiManager.enemies[i].GetComponent<TankMotor>();
                aiData = aiManager.enemies[i].GetComponent<TankData>();
                aiShoot = aiManager.enemies[i].GetComponent<TankShoot>();
                aiTrans = aiManager.enemies[i].GetComponent<Transform>();

            }

            
            
            int modeIndex = Random.Range(0, 3);
            switch (modeIndex)
            {
                case 0:
                    aiBehavior = Behavior.AGGRESSIVE;
                    aiMode = defaultMode = EnemyMode.CHASE;
                    break;
                case 1:
                    aiBehavior = Behavior.NERVOUS;
                    aiMode = defaultMode = EnemyMode.PATROL;
                    break;
                case 2:
                    aiBehavior = Behavior.TIMID;
                    defaultMode = EnemyMode.RUN;
                    aiMode = EnemyMode.PATROL;
                    break;              
            }
        }

        isForward = true;
        close = aiData.frontSpeed;



        
        for (int spawn = 0; spawn < aiManager.enemySpawns.Length; spawn++)
        {
            if (aiTrans.position == aiManager.enemySpawns[spawn].transform.position)
            {
                
                patrol = aiManager.patrols[spawn];
                int childCount = patrol.transform.childCount;
                patrolPath = new GameObject[childCount];

                for (int child = 0; child < patrolPath.Length; child++)
                {
                    patrolPath[child] = patrol.transform.GetChild(child).gameObject;
                }
            }
        }
       


       
        lastShot = Time.time;
        chaseRunTime = 0; 
        avoidingStep = 0;
        destination = 0;
        


    }
        

    
    void Update()
    {
        distanceToTarget = Vector3.Distance(aiTrans.position, chaseRunTarget);
        
        

        if (!CanMove())
        {            
            aiMode = EnemyMode.AVOIDING;
        }

        
        chaseRunTime -= Time.deltaTime;

        
        if (chaseRunTime <= 0)
        {
            if (defaultMode == EnemyMode.CHASE || defaultMode == EnemyMode.RUN)
            {
                targetIndex = Random.Range(0, aiManager.players.Length); 
                chaseRunTarget = aiManager.players[targetIndex].transform.position; 

            }

            if (aiBehavior == Behavior.NERVOUS)
            {
                for (int i = 0; i < (timeToChaseRun/2); i++)
                {
                    aiMotor.Turn(aiData.turnSpeed);
                }
                
                
            }
            chaseRunTime = timeToChaseRun;
        }

        if (aiData.health < aiData.maxHealth*.25)
        {
            aiMode = EnemyMode.REST;
        }

        

        switch (aiMode)
        {
            case EnemyMode.PATROL:
                
                Patrolling();
                break;
            case EnemyMode.CHASE:             
                Chasing(chaseRunTarget);
                break;            
            case EnemyMode.REST:
                Resting();
                break;
            case EnemyMode.RUN:                
                Running(chaseRunTarget);
                break;
            case EnemyMode.INVESTIGATE:
                
                break;
            case EnemyMode.AVOIDING:
                Avoid();
                break;
            default:
                break;
        }
    }


    public void Resting()
    {
        
        aiMotor.TurnTowards(GameObject.FindGameObjectWithTag("Rest").transform.position, aiData.turnSpeed);
        aiMotor.Move(aiData.frontSpeed);

        
        while (aiData.health < aiData.maxHealth)
        {
            aiData.health += 1;
        }

        
        if (aiData.health == aiData.maxHealth)
        {
            aiMode = defaultMode;
        }
    }
    public void Patrolling()
    {
       
        if (!(aiMotor.TurnTowards(patrolPath[destination].transform.position, aiData.turnSpeed)))
        {

            aiMotor.Move(aiData.frontSpeed);

            if (Vector3.SqrMagnitude(patrolPath[destination].transform.position - aiTrans.position) < (close * close))
            {
                

                if (destination == patrolPath.Length - 1)
                {
                    switch (pathway)
                    {

                        case Pathway.Loop:
                            
                            destination = 0;
                            break;
                        case Pathway.PingPong:
                            
                            isForward = false;
                            destination--;
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    
                    if (isForward)
                    {
                        destination++;
                    }
                    else
                    {
                        destination--;
                        if (destination == 0)
                        {
                            isForward = true;
                        }
                    }

                }

            }

        }
    }

    public void Running(Vector3 hostile)
    {
        
        Vector3 betweenTanks = hostile - aiTrans.position;

        
        Vector3 runDirection = -1 * betweenTanks;

        
        runDirection.Normalize();
        runDirection *= distanceToTarget;

        
        Vector3 runTarget = runDirection + aiTrans.position;
        aiMotor.TurnTowards(runTarget, aiData.turnSpeed);
        aiMotor.Move(aiData.frontSpeed);
    }

    public void Chasing(Vector3 target)
    {

        aiMotor.TurnTowards(target, aiData.turnSpeed);
        
        if (distanceToTarget < aiData.frontSpeed)
        {
            if (Time.time >= lastShot + aiData.fireRate)
            {
                aiShoot.Shoot(aiData.shellForce);
                lastShot = Time.time;
            }
        }

        aiMotor.Move(aiData.frontSpeed);
        
        
    }
    public bool CanMove()
    {
        start = aiTrans.position; 
        rtEndAngle = Quaternion.AngleAxis(angle, aiTrans.up) * aiTrans.forward; 
        lftEndAngle = Quaternion.AngleAxis(-angle, aiTrans.up) * aiTrans.forward; 

        
        front = new Ray(start, aiTrans.forward);
        rtFront = new Ray(start, rtEndAngle);
        lftFront = new Ray(start, lftEndAngle);

        
        if (Physics.Raycast(front, out hitInfo, aiData.frontSpeed))
        {
            Debug.DrawLine(start, hitInfo.point, Color.red);

            
            if (Physics.Raycast(rtFront, out hitInfo, aiData.frontSpeed))
            {
                Debug.DrawLine(start, hitInfo.point, Color.red);

                if (!hitInfo.collider.gameObject.CompareTag("Player"))
                {
                    avoidingStep = 1;
                }                
                else
                {
                    
                    chaseRunTarget = hitInfo.collider.gameObject.transform.position;
                    distanceToTarget = Vector3.Distance(chaseRunTarget, aiTrans.position);
                }
            }
            
            if (Physics.Raycast(lftFront, out hitInfo, aiData.frontSpeed))
            {
                Debug.DrawLine(start, hitInfo.point, Color.red);

                if (!hitInfo.collider.gameObject.CompareTag("Player"))
                {
                    avoidingStep = -1;
                }
                else
                {
                    
                }
            }
            
            return false;
        }
        else
        {
           
            avoidingStep = 0;
            return true;
        }

        
    }
    public void Avoid()
    {
        if (avoidingStep == 1)
        {
            aiMotor.Turn(-1 * aiData.turnSpeed);
        }
        else if (avoidingStep == -1)
        {
            aiMotor.Turn(1 * aiData.turnSpeed);
        }
        else if (avoidingStep == 0)
        {
            aiMode = defaultMode;
        }
        
    }

    public bool CanSee(GameObject target)
    {
        
        Vector3 vectorToTarget = target.transform.position - transform.position;

        
        float angleToTarget = Vector3.Angle(transform.forward, vectorToTarget);


        
        if (angleToTarget < fieldOfView)
        {

            
            RaycastHit hit;
            Ray ray = new Ray();
            ray.origin = transform.position; 
            ray.direction = vectorToTarget;  

           
            if (Physics.Raycast(ray, out hit, viewDistance))
            {
                if (hit.collider.gameObject.Equals(target))
                {
                    
                    return true;
                }
                else
                {
                    
                    return false;
                }

            }
            
            return false;

        }
        else
        {
            
            return false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        AudioSource noise = other.gameObject.GetComponent<AudioSource>();

        if (noise != null)
        {
            aiTrans.LookAt(noise.transform.position);

            if (CanSee(GameObject.FindGameObjectWithTag("Player")))
            {
                switch (aiBehavior)
                {
                    case Behavior.AGGRESSIVE:
                        aiShoot.Shoot(aiData.shellForce);
                        aiMode = EnemyMode.CHASE;
                        break;
                    case Behavior.NERVOUS:
                        aiShoot.Shoot(aiData.shellForce);
                        aiMode = EnemyMode.CHASE;
                        break;
                    case Behavior.TIMID:
                        aiShoot.Shoot(aiData.shellForce);
                        aiMode = EnemyMode.RUN;
                        break;
                    default:
                        break;
                }
                
            }


        }

    }


}