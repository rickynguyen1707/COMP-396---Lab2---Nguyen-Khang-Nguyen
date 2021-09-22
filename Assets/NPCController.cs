using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    //Simple FSM (FiniteStateMachine)
    public enum NPCState {
        Patrolling,
        Chasing,
        Attacking
    };


    public float CloseEnoughDistance = 2; //<=2 m => attack
    //To be able to simulate different states

    public NPCState currentState = NPCState.Patrolling;
    public bool bCanSeePlayer;
    [SerializeField]
    GameObject goPlayer;

    //New as of Sep.20th
    GameObject[] Waypoints;
    int CurrentWaypointIndex = 0;
    public float speed = 1; //1 m/s
    //--------------------------



    //float closenessToWaypoint=.1; //<.1 m => we are at waypoint
    void ChangeState(NPCState newState)
    {
        currentState = newState;
    }
    // Start is called before the first frame update
    void Start()
    {
        //First method: use tag Waypoint
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //Second method: get Waypoints' parent and then get all children (Waypoints)
    }

    // Update is called once per frame
    void Update()
    {
        HandleFSM();
        
    }
    void HandleFSM()
    {
        switch (currentState)
        {
            case NPCState.Patrolling:
                HandlePatrollingState();
                break;
            case NPCState.Chasing:
                HandleChasingState();
                break;
            case NPCState.Attacking:
                HandleAttackingState();
                break;
            default:
                break;
        }
    }

    private void HandleAttackingState()
    {
        Debug.Log("In NPCController.HandleAttackingState");
    }

    private void HandleChasingState()
    {
        //Debug.Log("In NPCController.HandleChasingState");


        if (!bCanSeePlayer)
        {
            ChangeState(NPCState.Patrolling);
        }

        this.transform.position = MyMoveTowards(this.transform.position, goPlayer.transform.position, speed * Time.deltaTime);


    }

    private void HandlePatrollingState()
    {
        Debug.Log("In NPCController.HandlePatrollingState");
        if (bCanSeePlayer)
        {
            ChangeState(NPCState.Chasing);
        }

        FollowPatrolingPath();
    }

    private void FollowPatrolingPath()
    {
        //if in waypoint => calculate nextwaypointindex
        //else, go to currentwaypointindex;

        //Mind different patrolling strategies
        // . follow in order
        // . follow randomly 
        Vector3 target = Waypoints[CurrentWaypointIndex].transform.position;
        if (Vector3.Distance(this.transform.position, target) < .1)
        {
            CurrentWaypointIndex = CalculateNextWaypointIndex();
            target=Waypoints[CurrentWaypointIndex].transform.position;
        }
        // speed m/s
        // d=v*dt  [m/s]*[s] => [m]
        //Vector3 movement = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        Vector3 movement = MyMoveTowards(this.transform.position, target, speed * Time.deltaTime);
        this.transform.position = movement;
        
    }

    Vector3 MyMoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        Vector3 C2T = target - current;

        //-------- Start of Changes related to Rotation -------------
        //this.transform.LookAt(target); //Abrupt1: this is a too abrupt rotation; see also Abrupt2 below

        Quaternion qtargetrotation = Quaternion.LookRotation(C2T);
        //this.transform.rotation = qtargetrotation;  //Abrupt2: this is a too abrupt rotation; not very beleivable; to try, uncomment this an comment out the line below

        // This is the smothest rotation; more beleivable
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, qtargetrotation, Time.deltaTime);
        //-------- End of Changes related to Rotation -------------

        Vector3 movement = current + C2T.normalized * maxDistanceDelta;
        return movement;

    }

    private int CalculateNextWaypointIndex()
    {
        //Strategy 1 - follow in order
        return CurrentWaypointIndex=(CurrentWaypointIndex+1) % Waypoints.Length;



 
        //throw new NotImplementedException();
    }

    private void CanSeeAdvesary()
    {
        //throw new NotImplementedException();

        //GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        //
        Vector3 playerPos = goPlayer.transform.position;
        Vector3 E2P_Heading = playerPos - this.transform.position;
        float cosAngleE2P = Vector3.Dot(this.transform.forward, E2P_Heading) / E2P_Heading.magnitude;
        //float cosAngleE2P = Vector3.Dot(this.transform.forward, E2P_Heading) ; //we need only the sign of cosAngle, so no need to devide by a >0 size (a small optimization)
        bCanSeePlayer = (cosAngleE2P > 0); //we are assuming FoV=180 degrees; if 
        
        
        float angle = Vector3.Angle(this.transform.forward, E2P_Heading);
        Debug.Log("angle=" + angle);
        //return bCanSeePlayer; //for testing purpuses



        //cos (theta)=v1.v2/(|v1|*|v2|)
        //if v1 is a unit vector => |v1|=1   //forward= (0,0,1) initially and it is always unit vector (with size 1)\
        //so are right, up;  left=-right, down=-up, back=-forward => they are also unit vectors
         //this.transform
    }

    private void FixedUpdate()
    {
        CanSeeAdvesary();
    }

    private void OnDrawGizmos()
    {   //Path
        Gizmos.color = Color.green;
        if (Waypoints.Length > 0) {
            for (int i = 0; i < Waypoints.Length; i++)
            {
                Vector3 from = Waypoints[i].transform.position;
                Vector3 to = Waypoints[(i + 1) % Waypoints.Length].transform.position;
                Gizmos.DrawLine(from, to);
            }
        }

    }
}
