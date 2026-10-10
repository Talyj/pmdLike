using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    public InputActionReference moveAction;
    private TurnManagement turn;
    public bool isMoving = false;
    Vector3 target;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        turn = FindAnyObjectByType<TurnManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        //0 = player turn
        if(turn.currentTurn == 0)
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
                    transform.position = target;
                    isMoving = false;
                    turn.TurnChange();
                }
            }
        }
    }

    private List<Vector3> GetMovePointAround()
    {
        List<Vector3> targetsPos = new List<Vector3>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);

        foreach(var pos in hitColliders)
        {
     
            if(pos.name == "center")
            {
                if (pos.transform.position.x == transform.position.x && pos.transform.position.z == transform.position.z) continue;
                targetsPos.Add(pos.transform.position);
            }
        }
        return targetsPos;
    }

}
