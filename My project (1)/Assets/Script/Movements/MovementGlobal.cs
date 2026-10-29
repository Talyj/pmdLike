using Assets.Script.Actions;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
public class MovementGlobal : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    public InputActionReference moveAction;
    private TurnManager turn;
    public bool isMoving = false;
    private UI ui;
    public Vector3 target;
    private int myTurn;

    public Vector3 lastPosition;
    private float maxMovement = 5f; // maybe go into statistique ?

    private List<GameObject> targetAround = new List<GameObject>();

    //Npc
    [HideInInspector] public bool hasMoved = false;
    [SerializeField] private NpcAction action;



    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        turn = FindAnyObjectByType<TurnManager>();
        myTurn = GetComponent<Statistiques>().myTurn;
        ui = FindAnyObjectByType<UI>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
    }

    public void MovementPlayer()
    {
        //Movement limité à x mètres
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        //Might remove that
        if (Input.GetButton("Look"))
        {
            transform.LookAt(transform.position + move);
        }
        else
        {
            if (Input.GetButtonDown("ResetPos"))
            {
                transform.position = lastPosition;
            }
            if (move == new Vector3()|| Vector3.Distance(transform.position, lastPosition) < maxMovement)
            {
                transform.LookAt(transform.localPosition + move);
                transform.localPosition += move * (Time.deltaTime * playerSpeed);
            }
            else
            {
                var dir = lastPosition - transform.position;
                transform.position += dir * (Time.deltaTime * playerSpeed);
            }

            //Validate
            //if input key truc
            if (Input.GetButtonDown("Jump"))
            {
                lastPosition = transform.position;

                //partie companion
                var npc = GetComponent<NpcController>();
                if (npc != null)
                {
                    turn.companionTurn++;
                    hasMoved = true;
                    GetComponent<NpcController>().isControlled = false;
                }

                //tous
                //transform.position = target;
                //isMoving = false;
                turn.TurnChange(gameObject);
            }

        }

    }

    public void Move(GameObject player)
    {
        var positions = GetClosestPointFromList(GetMovePointsAroundTarget(player));

        //var direction = (player.transform.position - transform.position).normalized;
        //var goTo = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);

        if (Vector3.Distance(transform.position, new Vector3( positions.x, turn.HeightMovementForEvryone, positions.z)) < 0.001f ||
            Vector3.Distance(transform.position, lastPosition) > maxMovement)
        {
            action.CheckForTargets();
            lastPosition = transform.position;
            hasMoved = true;
            isMoving = false;
            turn.companionTurn++;
            turn.TurnChange(gameObject);
        }
        else
        {
            MoveTo(positions);
        }
    }

    public void Wait()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, action.detectRange);

        foreach(var col in hitColliders)
        {
            if(col.tag.ToLower() == "player")
            {
                action.CheckForTargets();
                return;
            }
        }

        hasMoved = true;
        lastPosition = transform.position;
        isMoving = false;
        turn.TurnChange(gameObject);
    }




    private List<GameObject> GetTargetAround()
    {
        List<Vector3> targetsPos = new List<Vector3>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);

        var res = new List<GameObject>();
        foreach(var col in hitColliders)
        {
            if(col.tag.ToLower() == "enemy")
            {
                res.Add(col.gameObject);
            }
        }
        return res;
    }

    public List<Vector3> GetMovePointsAroundTarget(GameObject target)
    {
        List<Vector3> targetsPos = new List<Vector3>();
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, 2);
        RaycastHit hit;

        foreach(var pos in hitColliders)
        {
            var direction = new Vector3(pos.transform.position.x, transform.position.y, pos.transform.position.z) - transform.position;
            //CAnt go on a tile with an enemy in
            if (Physics.Raycast(transform.position, direction, out hit, 1.5f))
            {
                //If ici contact avec quelque chose

                //Peut traverser les companions?
                //if (hit.collider.tag == "characters")
                //{
                //    targetsPos.Add(pos.transform.position);
                //}
            }
            else if (pos.tag == "ground")
            {
                if (pos.transform.position.x == target.transform.position.x && pos.transform.position.z == target.transform.position.z) continue;
                targetsPos.Add(pos.transform.position);
            }
        }
        return targetsPos;
    }

    private Vector3 GetClosestPointFromList(List<Vector3> positions)
    {
        float dist = 100000;
        Vector3 res = new Vector3();
        //get closest to mo
        foreach (var pos in positions)
        {
            if (Vector3.Distance(transform.position, pos) < dist)
            {
                dist = Vector3.Distance(transform.position, pos);
                res = pos;
            }
        }
        return res;
    }

    //public Vector3 Diagonals(Vector3 move)
    //{
    //    Vector3 res = move;
    //    if(move.x != 0 && move.z != 0)
    //    {
    //        res = new Vector3(move.x / Math.Abs(move.x), 0, move.z / Math.Abs(move.z));
    //    }
    //    return res;
    //}

    public void MoveTo(Vector3 targetPos)
    {
        isMoving = true;
        var direction = new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z) - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z), playerSpeed * Time.deltaTime * 2);
    }

    public void Movement()
    {
        ////If is moving cant input something else
        //if (!isMoving)
        //{
        //    List<Vector3> possibleTargets = GetMovePointAround();

        //    // Read input
        //    Vector2 input = moveAction.action.ReadValue<Vector2>();
        //    Vector3 move = new Vector3(input.x, 0, input.y);
        //    move = Vector3.ClampMagnitude(move, 1f);
        //    move = Diagonals(move);

        //    //Fire1 = left shift
        //    if (Input.GetButton("Look"))
        //    {
        //        //Maybe have to change that
        //        if(GetTargetAround().Count <= 0)
        //        {
        //            transform.LookAt(transform.position + move);
        //        }
        //        else
        //        {

        //            targetAround = GetTargetAround();
        //            var rdm = UnityEngine.Random.Range(0, targetAround.Count);

        //            transform.LookAt(targetAround[rdm].transform.position);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var pos in possibleTargets)
        //        {
        //            if (transform.position.x + move.x == pos.x && transform.position.z + move.z == pos.z)
        //            {
        //                target = new Vector3(pos.x, turn.HeightMovementForEvryone, pos.z);
        //                isMoving = true;
        //                break;
        //            }
        //        }

        //        if (move != Vector3.zero)
        //            transform.forward = move;
        //    }

        //    //THis works with the controller
        //    //var test = Input.GetAxis("Horizontal");
        //    //var test2 = Input.GetAxis("Vertical");
        //    //Vector2 input = new Vector2(test, test2);
        //}
        //float step = playerSpeed * Time.deltaTime;

        //if(transform.position != target)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, target, step);
        //    if(Vector3.Distance(transform.position, target) < 0.001f)
        //    {
        //        //partie companion
        //        var npc = GetComponent<NpcMovement>();
        //        if(npc != null)
        //        {
        //            turn.companionTurn++;
        //            npc.hasMoved = true;
        //            GetComponent<NpcController>().isControlled = false;
        //        }

        //        //tous
        //        transform.position = target;
        //        isMoving = false;
        //        turn.TurnChange();
        //    }
        //}
    }

}
