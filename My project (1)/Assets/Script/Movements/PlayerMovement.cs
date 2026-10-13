using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    public InputActionReference moveAction;
    private TurnManager turn;
    public bool isMoving = false;
    private UI ui;
    Vector3 target;
    private int myTurn;

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

            foreach(var pos in possibleTargets)
            {
                if (transform.position.x + move.x == pos.x && transform.position.z + move.z == pos.z)
                {
                    target = new Vector3(pos.x, 3.5f, pos.z);
                    isMoving = true;
                    break;
                }
            }

            if (move != Vector3.zero)
                transform.forward = move;

        }
        float step = playerSpeed * Time.deltaTime;

        if(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if(Vector3.Distance(transform.position, target) < 0.001f)
            {
                var npc = GetComponent<NpcMovement>();
                if(npc != null)
                {
                    turn.companionTurn++;
                    npc.hasActed = true;
                }
                if (turn.modeDeCombat == 1)
                {
                    ui.GetNewPlayerController();
                }

                transform.position = target;
                isMoving = false;
                turn.TurnChange();
            }
        }
    }

    private List<Vector3> GetMovePointAround()
    {
        List<Vector3> targetsPos = new List<Vector3>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
        RaycastHit hit;

        foreach(var pos in hitColliders)
        {
            var direction = new Vector3(pos.transform.position.x, transform.position.y, pos.transform.position.z) - transform.position;
            if(Physics.Raycast(transform.position, direction, out hit, 1f))
            {
                if(hit.collider.tag == "characters")
                {
                    targetsPos.Add(pos.transform.position);
                }
            }
            else if (pos.name.Contains("Ground"))
            {
                if (pos.transform.position.x == transform.position.x && pos.transform.position.z == transform.position.z) continue;
                targetsPos.Add(pos.transform.position);
            }
        }
        return targetsPos;
    }
}
