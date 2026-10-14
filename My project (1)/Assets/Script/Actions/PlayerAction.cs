using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using static UnityEngine.GraphicsBuffer;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerMovement move;
    //[HideInInspector] public PlayerController playerCont;
    private TurnManager turn;
    private NpcController npc;
    private bool isActing = false;
    private PlayerFollow mainCamera;

    private int myTurn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTurn = GetComponent<Statistiques>().myTurn;
        turn = FindAnyObjectByType<TurnManager>();
        npc = GetComponent<NpcController>();
        mainCamera = FindAnyObjectByType<PlayerFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();
    }

    public void Attack()
    {
        //If is moving cant input something else
        if (!isActing)
        {
            //Jump = space bar
            if (Input.GetButtonUp("Jump") && mainCamera.targetPosition == gameObject)
            {
                isActing = true;
                Debug.Log("iam : " + tag);
                DealAOEDamages(50, 2f);

                //The change turn must be in another if ?
                ChangeNpcState();
                isActing = false;
                turn.TurnChange();
            }
        }
    }

    private bool ChangeNpcState()
    {
        npc = GetComponent<NpcController>();
        if (npc != null)
        {
            turn.companionTurn++;
            GetComponent<NpcMovement>().hasMoved = true;
        }
        return false;
    }

    private void DealAOEDamages(int value, float range)
    {
        move.isMoving = false;        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hitColliders)
        {
            if (hit.tag == "enemy")
            {
                hit.GetComponent<Statistiques>().TakeDamage(value);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //    Gizmos.DrawWireSphere(transform.position, 2f);
    //}
}
