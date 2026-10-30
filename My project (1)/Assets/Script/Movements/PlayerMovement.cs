using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    public InputActionReference moveAction;
    private TurnManager turn;
    public bool isMoving = false;
    private UI ui;
    public Vector3 target;
    private int myTurn;

    private List<GameObject> targetAround = new List<GameObject>();

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
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
    }

    public void Movement()
    {
        //If is moving cant input something else
        if (!isMoving)
        {
            List<Vector3> possibleTargets = GetMovePointAround();

            // Read input
            Vector2 input = moveAction.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = Vector3.ClampMagnitude(move, 1f);
            move = Diagonals(move);

            //Fire1 = left shift
            if (Input.GetButton("Fire1"))
            {
                //Maybe have to change that
                if(GetTargetAround().Count <= 0)
                {
                    transform.LookAt(transform.position + move);
                }
                else
                {

                    targetAround = GetTargetAround();
                    var rdm = UnityEngine.Random.Range(0, targetAround.Count);

                    transform.LookAt(targetAround[rdm].transform.position);
                }
            }
            else
            {
                foreach (var pos in possibleTargets)
                {
                    if (transform.position.x + move.x == pos.x && transform.position.z + move.z == pos.z)
                    {
                        target = new Vector3(pos.x, turn.HeightMovementForEvryone, pos.z);
                        isMoving = true;
                        break;
                    }
                }

                if (move != Vector3.zero)
                    transform.forward = move;
            }

            //THis works with the controller
            //var test = Input.GetAxis("Horizontal");
            //var test2 = Input.GetAxis("Vertical");
            //Vector2 input = new Vector2(test, test2);
        }
        float step = playerSpeed * Time.deltaTime;

        if(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if(Vector3.Distance(transform.position, target) < 0.001f)
            {
                //partie companion
                var npc = GetComponent<NpcMovement>();
                if(npc != null)
                {
                    turn.companionTurn++;
                    npc.hasMoved = true;
                    GetComponent<NpcController>().isControlled = false;
                }

                //tous
                transform.position = target;
                isMoving = false;
                turn.TurnChange();
            }
        }
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

    public List<Vector3> GetMovePointAround()
    {
        List<Vector3> targetsPos = new List<Vector3>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
        RaycastHit hit;

        foreach(var pos in hitColliders)
        {
            var direction = new Vector3(pos.transform.position.x, transform.position.y, pos.transform.position.z) - transform.position;
            //CAnt go on a tile with an enemy in
            if (Physics.Raycast(transform.position, direction, out hit, 1f))
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
                if (pos.transform.position.x == transform.position.x && pos.transform.position.z == transform.position.z) continue;
                targetsPos.Add(pos.transform.position);
            }
        }
        return targetsPos;
    }

    public Vector3 Diagonals(Vector3 move)
    {
        Vector3 res = move;
        if(move.x != 0 && move.z != 0)
        {
            res = new Vector3(move.x / Math.Abs(move.x), 0, move.z / Math.Abs(move.z));
        }
        return res;
    }

    public void MoveTo(Vector3 targetPos)
    {
        isMoving = true;
        var direction = new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z) - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, turn.HeightMovementForEvryone, targetPos.z), playerSpeed * Time.deltaTime * 5);
    }
}
