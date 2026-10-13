using Assets.Script.Actions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor.Android;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class NpcMovement : MonoBehaviour
{
    private float height = 3.5f;
    private TurnManagement turn;

    [SerializeField] private float playerSpeed;
    public InputActionReference moveAction;
    [SerializeField]public bool isCompanion = true;
    public bool hasActed = false;


    //Player variables (companion)
    private PlayerMovement player;
    private Vector3 targetPos;

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
        //C'était dans start
        if (isCompanion)
        {
            player = Object.FindAnyObjectByType<PlayerMovement>();
            targetPos = player.transform.position;
        }
        else
        {
            rdmTargetPos = GetRandomTile(GetAroundTile());
            posGround = transform.position;
        }

        turn = FindAnyObjectByType<TurnManagement>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //    Gizmos.DrawWireSphere(transform.position + transform.position, 1.5f);
    //}


    public void FollowLeader()
    {
        if(isCompanion && turn.currentTurn == 1)
        {
            //Base behavior for a companion
            //if it's my turn then i follow
            if(transform.position != targetPos && hasActed == false)
            {
                //Ajouter un genre de pathfinding pour retrouver le joueur

                if(Vector3.Distance(transform.position, targetPos) > 1.5)
                {
                    Debug.Log("wsh t'es loin");
                    //TargetMove();
                }
                //Check if the target tile is empty
                if (Physics.Raycast(transform.position, targetPos - transform.position, out hit, .5f))
                {
                    if (hit.collider.name.Contains("Body"))
                    {
                        targetPos = lastPosition;
                    }

                }
                var direction = targetPos - transform.position;
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSpeed * Time.deltaTime);           
            }
            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                lastPosition = transform.position;
                targetPos = player.transform.position;
                hasActed = true;
                turn.TurnChange();
            }
        }
    }

    void Attack()
    {
        //Add the behavior of a violent companion
    }

    void flee()
    {
        //Add the behavior of a wimpy companion
    }

    public void EnemiesMove()
    {
        if (!isCompanion && turn.currentTurn == 2)
        {
            if (!isMoving)
            {
                action.Attack();
            }
            if (!action.hasTarget)
            {
                RandomMove();
            }
            else
            {
                TrackingMove();
            }
        }
    }

    void TrackingMove()
    {
        posGround = MoveToTarget(posGround);
        if (Vector3.Distance(transform.position, new Vector3(posGround.x, height, posGround.z)) < 0.001f)
        {
            hasActed = true;
            //Check for targets around (player/ companion)
            if (!action.CheckForTargets())
            {
                //reset target if out of range
                action.ResetTargets();
            }
            else
            {
                posGround = GetClosestTileToTarget(GetAroundTile()).transform.position;
            }
            lastPosition = transform.position;
            isMoving = false;
            turn.TurnChange();
        }
    }

    void RandomMove()
    {
        rdmTargetPos = MoveToTarget(rdmTargetPos);
        if (Vector3.Distance(transform.position, new Vector3(rdmTargetPos.x, height, rdmTargetPos.z)) < 0.001f)
        {
            hasActed = true;
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
            lastPosition = transform.position;
            isMoving = false;
            turn.TurnChange();
        }
    }

    public Ground GetClosestTileToTarget(List<Ground> grounds)
    {
        float dist = 100000;
        Ground res = null;
        if (grounds.Count() > 0)
        {
            if (action.player != null)
            {
                foreach (var grd in grounds)
                {
                    if (Vector3.Distance(action.player.transform.position, grd.transform.position) < dist)
                    {
                        dist = Vector3.Distance(action.player.transform.position, grd.transform.position);
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
                if(new Vector3(col.transform.position.x, height, col.transform.position.z) != transform.position)
                {
                    res.Add(col.GetComponent<Ground>());
                }              
            }                
        }
        return res;
    }

    private Vector3 MoveToTarget(Vector3 targetPos)
    {
        if (transform.position != new Vector3(targetPos.x, height, targetPos.z) && !hasActed)
        {
            //if target too far
            if (Vector3.Distance(transform.position, targetPos) > 1.5)
            {
                //check the distance
                targetPos = GetRandomTile(GetAroundTile());
            }        //Check if the target tile is empty
            if (Physics.Raycast(transform.position, targetPos - transform.position, out hit, 1f))
            {
                if (hit.collider.name.Contains("Body"))
                {
                    //Debug.Log("NOOOOOOOOOOOOOOOOOO");
                    targetPos = lastPosition;
                }

            }
            isMoving = true;
            //rdmTargetPos = new Vector3(groundTargets[integer].transform.position.x, height, groundTargets[integer].transform.position.z);
            var direction = new Vector3(targetPos.x, height, targetPos.z) - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, height, targetPos.z), playerSpeed * Time.deltaTime);
        }
        return targetPos;
    }
}
