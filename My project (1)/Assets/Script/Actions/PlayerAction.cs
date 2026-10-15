using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
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

    public void Attack()
    {
        //If is moving cant input something else
        if (!isActing)
        {
            isActing = true;
            DealAOEDamages(50, 2f);

            //The change turn must be in another if ?
            ChangeNpcState();
            isActing = false;
            turn.TurnChange();
        }
    }

    public void SimpleAttack(float range, int dmg)
    {
        //Race skill should be put there like
        //if(race == gluant){
        //Change something
        //}
        //else if(race == goblin){
        // change something else
        //}

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if(hit.collider.tag.ToLower() == "enemy")
            {
                hit.collider.GetComponent<Statistiques>().TakeDamage(dmg);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public IEnumerator Grab(float range ,int dmg)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if(hit.collider.tag.ToLower() == "enemy")
            {
                var target = hit.collider.gameObject;
                var direction = (hit.collider.transform.position - transform.position).normalized;
                var grabTo = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
                while (target.transform.position != grabTo)
                {
                    target.GetComponent<NpcMovement>().MoveTo(grabTo);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public void Heal(float range, int heal)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.collider.tag.ToLower() == "characters" || hit.collider.tag.ToLower() == "player")
            {
                hit.collider.GetComponent<Statistiques>().TakeDamage(-heal);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public void HealAround(float range, int heal)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hitColliders)
        {
            if (hit.tag.ToLower() == "player" || hit.tag.ToLower() == "characters")
            {
                //-heal because flemme de faire deux fonctions
                hit.GetComponent<Statistiques>().TakeDamage(-heal);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    private void DealAOEDamages(int dmg, float range)
    {
        move.isMoving = false;        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hitColliders)
        {
            if (hit.tag == "enemy")
            {
                hit.GetComponent<Statistiques>().TakeDamage(dmg);
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

}
