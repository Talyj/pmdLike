using Assets.Script.Actions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Android;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class NpcMovement : MonoBehaviour
{
    [HideInInspector]public TurnManager turn;

    [SerializeField] private float playerSpeed;
    [HideInInspector] public bool hasMoved = false;
    private int myTurn;


    //Player variables (companion)
    [HideInInspector] public Statistiques targetStats;
    [SerializeField] private Vector3 targetPos;
    private NpcController npcController;

    //Randomness variables (enemy)
    private List<Ground> grounds = new List<Ground>();
    [SerializeField]private Vector3 rdmTargetPos;
    private Vector3 lastPosition;
    RaycastHit hit;
    public Vector3 posGround;
    public bool isMoving = false;

    //Action variables
    [SerializeField] private NpcAction action;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcController = GetComponent<NpcController>();
        if (npcController.isCompanion)
        {
            targetPos = targetStats.transform.position;
        }
        else
        {
            rdmTargetPos = GetRandomTile(GetAroundTile());
            posGround = transform.position;
        }

        myTurn = GetComponent<Statistiques>().myTurn;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemiesMove()
    {
        if (!npcController.isCompanion && turn.currentTurn == myTurn)
        {
            if (action.hasTarget && action.target != null)
            {
                if(Vector3.Distance(action.target.transform.position, transform.position) <= action.atqRange && !isMoving)
                {
                    action.Attack();
                }
                else
                {
                    TrackingMove();
                }
            }
            else
            {
                RandomMove();
            }
        }
    }

    public void FollowLeader()
    {
        if(npcController.isCompanion && turn.currentTurn == myTurn && turn.companionTurn == npcController.orderId)
        {
            TrackingFollowCompanion();
        }
    }

    void TrackingFollowCompanion()
    {
        targetPos = MoveToTarget(targetPos, 1.5f);
        if (Vector3.Distance(transform.position, new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z)) < 0.001f)
        {
            hasMoved = true;
            targetPos = GetClosestTileToTarget(GetAroundTile(), targetStats.transform.position, targetStats).transform.position;
            lastPosition = transform.position;
            turn.companionTurn++;
            isMoving = false;
            turn.TurnChange();
        }
    }

    void TrackingMove()
    {
        posGround = MoveToTarget(posGround, 1.5f);
        if (Vector3.Distance(transform.position, new Vector3(posGround.x, turn.HeightMovementForEvryone, posGround.z)) < 0.001f)
        {
            //Check for targets around (player/ companion)
            if (!action.CheckForTargets())
            {
                //reset target if out of range
                action.ResetTargets();
            }
            else
            {
                posGround = GetClosestTileToTarget(GetAroundTile(), action.target.transform.position, action.target).transform.position;
            }
            hasMoved = true;
            lastPosition = transform.position;
            isMoving = false;
            turn.TurnChange();
        }
    }

    void RandomMove()
    {
        rdmTargetPos = MoveToTarget(rdmTargetPos, 1.5f);
        if (Vector3.Distance(transform.position, new Vector3(rdmTargetPos.x, turn.HeightMovementForEvryone, rdmTargetPos.z)) < 0.001f)
        {
            //Check for targets around (player/ companion)
            if (!action.CheckForTargets())
            {
                //reset target if out of range
                action.ResetTargets();
                //serach for a tile next to it
                rdmTargetPos = GetRandomTile(GetAroundTile());
            }
            else
            {
                //Move towards the target
            }
            hasMoved = true;
            lastPosition = transform.position;
            isMoving = false;
            turn.TurnChange();
        }
    }

    public Ground GetClosestTileToTarget(List<Ground> grounds, Vector3 targetPos, Statistiques target)
    {
        float dist = 100000;
        Ground res = null;
        if (grounds.Count() > 0)
        {
            if (target != null)
            {
                foreach (var grd in grounds)
                {
                    if (Vector3.Distance(targetPos, grd.transform.position) < dist)
                    {
                        dist = Vector3.Distance(targetPos, grd.transform.position);
                        res = grd;
                    }
                }
            }
        }
        return res;
    }

    private Vector3 GetRandomTile(List<Ground> grounds)
    {
        int rdm = Random.Range(0, grounds.Count);
        if(grounds.Count() > 0)
        {
            if (grounds[rdm].IsEmpty)
            {
                return grounds[rdm].transform.position;
            }
        }
        return transform.position;
    }

    public List<Ground> GetAroundTile()
    {
        List<Ground> res = new List<Ground>();

        //Get the collider around to see where i can go
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

        foreach(var col in hitColliders)
        {
            var direction = new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z) - transform.position;
            if (col.tag.ToLower() == "ground")
            {
                //Take only the tiles that are not me
                if (new Vector3(col.transform.position.x, turn.HeightMovementForEvryone, col.transform.position.z) != transform.position)
                {
                    //Debug.Log("my place");
                    res.Add(col.GetComponent<Ground>());
                }
            }                
        }
        return res;
    }

    private Vector3 MoveToTarget(Vector3 targetPos, float range)
    {
        if (transform.position != new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z) && !hasMoved)
        {
            //if target too far
            if (Vector3.Distance(transform.position, targetPos) > 1.5)
            {
                //check the distance if the value is weird just take a random tile next to it
                targetPos = GetRandomTile(GetAroundTile());
            }   //Check if the target tile is empty
            if (Physics.Raycast(transform.position, targetPos - transform.position, out hit, range))
            {
                if (hit.collider.tag.ToLower() != "ground")
                {
                    //Debug.Log("NOOOOOOOOOOOOOOOOOO");
                    targetPos = lastPosition;
                }

            }
            isMoving = true;
            //rdmTargetPos = new Vector3(groundTargets[integer].transform.position.x, height, groundTargets[integer].transform.position.z);

            var direction = new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z) - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z), playerSpeed * Time.deltaTime);

        }
        return targetPos;
    }
}
