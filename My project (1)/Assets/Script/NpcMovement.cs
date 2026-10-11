using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


    //Player variables
    private PlayerController player;
    private Vector3 targetPos;

    //Randomness variables
    private List<Ground> groundTargets = new List<Ground>();
    [SerializeField]private Vector3 rdmTargetPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isCompanion)
        {
            player = Object.FindAnyObjectByType<PlayerController>();
            targetPos = player.transform.position;
        }
        else
        {
            var grounds = GetAroundTile();
            foreach(var obj in grounds)
            {
                if(obj.name.Contains("Ground"))
                {
                    groundTargets.Add(obj);

                }
            }
            Debug.Log(transform.position);
            rdmTargetPos = ChooseRandomTile(groundTargets);
        }

        turn = FindAnyObjectByType<TurnManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isCompanion && turn.currentTurn == 1)
        {
            FollowLeader();
        }
        else if(!isCompanion && turn.currentTurn == 2)
        {
            RandomMove();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //    Gizmos.DrawWireSphere(transform.position + transform.position, 1.5f);
    //}


    void FollowLeader()
    {
        //Base behavior for a companion
        //if it's my turn then i follow
        if(transform.position != targetPos && hasActed == false)
        {
            //Before moving check the distance between me and the target, if it's too far change the targetPos to a closer tile
            if(Vector3.Distance(transform.position, targetPos) > 1.5)
            {
                //Get the collider around to see where i can go
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
                Collider closestCollider = new Collider();
                float distCollider = 2;

                //get direction to get to player
                //Vector3 direction = targetPos - transform.position;

                //check every collider named "BodyPlayer" to not move if it's next to it
                foreach (var pos in hitColliders)
                {
                    if(pos.name == "BodyPlayer")
                    {
                        targetPos = transform.position;
                        break;
                    }
                    else if(pos.name == "center")
                    {
                        if(Vector3.Distance(targetPos, pos.transform.position) < distCollider)
                        {
                            distCollider = Vector3.Distance(targetPos, pos.transform.position);
                            targetPos = new Vector3(pos.transform.position.x, 0.5f, pos.transform.position.x);
                        }
                    }
                    //targetPos = transform.position + direction.normalized;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {              
            targetPos = player.transform.position;
            hasActed = true;
            turn.TurnChange();
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

    void RandomMove()
    {
        //Base behavior for an enemy npc
        if (transform.position != new Vector3(rdmTargetPos.x, height, rdmTargetPos.z) && !hasActed)
        {
            //Before moving check the distance between me and the target, if it's too far change the targetPos to a closer tile
            //if (Vector3.Distance(transform.position, rdmTargetPos) > 1.5)
            //{

            //    //Get the collider around to see where i can go
            //    //Collider[] hitColliders = Physics.OverlapSphere(transform.transform.position, 1.5f);
            //    //Collider closestCollider = new Collider();
            //    float distCollider = Vector3.Distance(rdmTargetPos, transform.position);

            //    //get the closest tile in direction of the target
            //    //check every collider named "BodyPlayer" to not move if it's next to it
            //    foreach (var pos in GetAroundTile())
            //    {
            //        //if (pos.name == "BodyPlayer")
            //        //{
            //        //    rdmTargetPos = transform.position;
            //        //    break;
            //        //}
            //        //else

            //        if (Vector3.Distance(rdmTargetPos, pos.transform.position) < distCollider)
            //        {
            //            distCollider = Vector3.Distance(rdmTargetPos, pos.transform.position);
            //            rdmTargetPos = new Vector3(pos.transform.position.x, height, pos.transform.position.x);
            //        }

            //    }
            //}
            //Check if the target is empty TODO?
            //groundTargets = GetAroundTile();
            //var integer = Random.Range(0, groundTargets.Count() - 1);

            //Make the limited walking distance work
            //if (Vector3.Distance(transform.position, rdmTargetPos) > 1.5)
            //{
            //    //check the distance
            //    rdmTargetPos = MaketheMoveMoveable(transform.position, rdmTargetPos);
            //}

            //rdmTargetPos = new Vector3(groundTargets[integer].transform.position.x, height, groundTargets[integer].transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(rdmTargetPos.x, height, rdmTargetPos.z), playerSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, new Vector3(rdmTargetPos.x, height, rdmTargetPos.z)) < 0.001f)
        {
            hasActed = true;
            //serach for a tile next to it
            rdmTargetPos = ChooseRandomTile(groundTargets);
            turn.TurnChange();
        }
    }

    private Vector3 ChooseRandomTile(List<Ground> grounds)
    {
        int rdm = Random.Range(0, grounds.Count);
        //While the choosen tile is the same as i'm already on
        while(grounds[rdm].transform.position.x == transform.position.x && grounds[rdm].transform.position.z == transform.position.z)
        {
            rdm = Random.Range(0, grounds.Count);
        }
        if (grounds[rdm].IsEmpty)
        {
            return grounds[rdm].transform.position;
        }
        return transform.position;
    }

    private Vector3 MaketheMoveMoveable(Vector3 myPos, Vector3 targetPos)
    {
        float x=0, z = 0;
        if(Mathf.Abs(myPos.x - targetPos.x) > 1.5)
        {
            x = targetPos.x -1;
        }
        if(Mathf.Abs(myPos.z - targetPos.z) > 1.5)
        {
            z = targetPos.z -1;

        }
        return new Vector3(x, height, z);
    }

    private List<Ground> GetAroundTile()
    {
        List<Ground> res = new List<Ground>();

        //Get the collider around to see where i can go
        Collider[] hitColliders = Physics.OverlapSphere(transform.transform.position, 1.5f);
        Collider closestCollider = new Collider();
        foreach(var col in hitColliders)
        {
            if (col.name.Contains("Ground") && transform.position.x != col.transform.position.x && transform.position.z != col.transform.position.z)
            {
                res.Add(col.GetComponent<Ground>());
            }
        }
        //float distCollider = Vector3.Distance(rdmTargetPos, transform.position);

        return res;
    }
}
