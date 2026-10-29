using Assets.Script.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using static TurnManager;
using static UnityEngine.GraphicsBuffer;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private MovementGlobal move;
    [SerializeField] private Statistiques stats;
    //[HideInInspector] public PlayerController playerCont;
    [HideInInspector] public TurnManager turn;
    private NpcController npc;
    public bool isActing = false;
    [HideInInspector]public PlayerFollow mainCamera;
    public List<string> skillsList = new List<string>();

    private int myTurn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTurn = GetComponent<Statistiques>().myTurn;
        turn = FindAnyObjectByType<TurnManager>();
        npc = GetComponent<NpcController>();
        mainCamera = FindAnyObjectByType<PlayerFollow>();
        move = GetComponent<MovementGlobal>();
        stats = GetComponent<Statistiques>();
    }

    public void TestBehavior(List<string> skillList)
    {
        foreach(var test in skillList)
        {
            //assign the skill to a button
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
        turn.TurnChange(gameObject);
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
                    target.GetComponent<MovementGlobal>().MoveTo(grabTo);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange(gameObject);
    }

    public IEnumerator Grapple(float range, int dmg)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            var target = hit.collider.gameObject;
            var direction = -(hit.collider.transform.position - transform.position).normalized;

            var grabTo = new Vector3(target.transform.position.x + direction.x, target.transform.position.y + direction.y, target.transform.position.z + direction.z);
            while (transform.position != grabTo)
            {
                move.MoveTo(grabTo);
                if (Vector3.Distance(transform.position, grabTo) < 0.001f)
                {
                    transform.position = grabTo;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        ChangeNpcState();
        isActing = false;
        move.target = transform.position;
        move.isMoving = false;
        turn.TurnChange(gameObject);
    }

    public void Heal(float range, int heal)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (/*hit.collider.tag.ToLower() == "characters" ||*/ hit.collider.tag.ToLower() == "player")
            {
                hit.collider.GetComponent<Statistiques>().TakeDamage(-heal);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange(gameObject);
    }

    public void HealAround(float range, int heal)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hitColliders)
        {
            if (hit.tag.ToLower() == "player" /*|| hit.tag.ToLower() == "characters"*/)
            {
                //-heal because flemme de faire deux fonctions
                hit.GetComponent<Statistiques>().TakeDamage(-heal);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange(gameObject);
    }

    public void DealAOEDamages(float range, int dmg)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, 3f);
            foreach (var col in hitColliders)
            {
                if (col.tag == "enemy")
                {
                    col.GetComponent<Statistiques>().TakeDamage(dmg);
                }
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange(gameObject);
    }

    public void Aggro(float range, int dmg)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach(var col in hitColliders)
        {
            if(col.tag.ToLower() == "enemy")
            {
                col.GetComponent<NpcAction>().target = stats;
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange(gameObject);
    }

    public void PoisonAttack(float range, int duration)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, 3f);
            foreach (var col in hitColliders)
            {
                if (col.tag.ToLower() == "enemy" /*|| col.tag.ToLower() == "characters"*/ || col.tag.ToLower() == "player")
                {
                    if (!turn.GetComponent<DamageOverTime>().listCharacterToDebuff.ContainsKey(col.gameObject))
                    {
                        turn.GetComponent<DamageOverTime>().listCharacterToDebuff.Add(col.gameObject, duration);
                    }
                }
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange(gameObject);
    }

    public bool ChangeNpcState()
    {
        npc = GetComponent<NpcController>();
        if (npc != null)
        {
            turn.companionTurn++;
            GetComponent<MovementGlobal>().hasMoved = true;
        }
        return false;
    }

    //Add a function that does the overlapsphere (too much occurence)

}

//public class simpleattack :MonoBehaviour
//{
//    private PlayerAction action;

//    private void Start()
//    {
//        action = GetComponent<PlayerAction>();
//    }
//    public void SimpleAttack(float range, int dmg)
//    {
//        //Race skill should be put there like
//        //if(race == gluant){
//        //Change something
//        //}
//        //else if(race == goblin){
//        // change something else
//        //}

//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
//        {
//            if (hit.collider.tag.ToLower() == "enemy")
//            {
//                hit.collider.GetComponent<Statistiques>().TakeDamage(dmg);
//            }
//        }
//        action.ChangeNpcState();
//        action.isActing = false;
//        action.turn.TurnChange();
//    }
//}
