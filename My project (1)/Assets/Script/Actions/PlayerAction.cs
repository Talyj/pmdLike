using Assets.Script.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using Debug = UnityEngine.Debug;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerMovement move;
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
        move = GetComponent<PlayerMovement>();
        stats = GetComponent<Statistiques>();
    }

    public void SimpleAttack(float range, float dmg)
    {
        //dmg calc simpleattack = 100% des dégâts
        dmg *= 1;


        //Race skill should be put there like
        //if(race == gluant){
        //Change something
        //}
        //else if(race == goblin){
        // change something else
        //}
        //Debug.Log("simpleattack" + gameObject.name);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if(CheckIfTagIsDifferent(hit.collider.tag))
            {
                hit.collider.GetComponent<Statistiques>().TakeDamage(dmg);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public IEnumerator Grab(float range ,float dmg)
    {
        //dmg calc grab = 5% des dégâts
        dmg *= .05f;

        //Debug.Log("Grab" + gameObject.name);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            //if(hit.collider.tag.ToLower() == "enemy")
            //{
            var target = hit.collider.gameObject;
            var direction = (new Vector3(hit.collider.transform.position.x, turn.HeightMovementForEvryone, hit.collider.transform.position.z) - transform.position).normalized;
            var grabTo = new Vector3(transform.position.x + direction.x, turn.HeightMovementForEvryone + direction.y, transform.position.z + direction.z);
            while (target.transform.position != grabTo)
            {
                target.GetComponent<PlayerMovement>().MoveTo(grabTo);
                yield return new WaitForEndOfFrame();
            }
            //}
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public IEnumerator Grapple(float range, float dmg)
    {
        //dmg calc simpleattack = 5% des dégâts
        dmg *= .05f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            var target = hit.collider.gameObject;
            var direction = -(new Vector3(hit.collider.transform.position.x, turn.HeightMovementForEvryone, hit.collider.transform.position.z) - transform.position).normalized;

            var grabTo = new Vector3(target.transform.position.x + direction.x, turn.HeightMovementForEvryone + direction.y, target.transform.position.z + direction.z);
            //while (transform.position != grabTo)
            while (Vector3.Distance(transform.position, grabTo) > 0.001f)
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
        turn.TurnChange();
    }

    public void Heal(float range, float heal)
    {
        //dmg calc heal = 80% des dégâts
        heal *= .8f;

        //Debug.Log("Heal" + gameObject.name);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (/*hit.collider.tag.ToLower() == "characters" ||*/ hit.collider.tag.ToLower() == tag.ToLower())
            {
                hit.collider.GetComponent<Statistiques>().TakeDamage(-heal);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public void HealAround(float range, float heal)
    {
        //dmg calc healArea = 50% des dégâts
        heal *= .5f;

        //Debug.Log("HealAround" + gameObject.name);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hitColliders)
        {
            if (hit.tag.ToLower() == tag.ToLower() /*|| hit.tag.ToLower() == "characters"*/)
            {
                //-heal because flemme de faire deux fonctions
                hit.GetComponent<Statistiques>().TakeDamage(-heal);
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public void DealAOEDamages(float range, float dmg)
    {
        //dmg calc dmg = 50% des dégâts
        dmg *= .5f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, 3f);
            foreach (var col in hitColliders)
            {
                if (CheckIfTagIsDifferent(col.tag))
                {
                    col.GetComponent<Statistiques>().TakeDamage(dmg);
                }
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public void Aggro(float range, float dmg)
    {
        //dmg calc dmg = 10% des dégâts
        dmg *= .1f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach(var col in hitColliders)
        {
            if(CheckIfTagIsDifferent(col.tag))
            {
                col.GetComponent<NpcAction>().target = stats;
            }
        }
        ChangeNpcState();
        isActing = false;
        turn.TurnChange();
    }

    public void PoisonAttack(float range, int duration)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, 3f);
            foreach (var col in hitColliders)
            {
                if (col.tag.ToLower() == "enemy" || col.tag.ToLower() == "player")
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
        turn.TurnChange();
    }

    public bool ChangeNpcState()
    {
        npc = GetComponent<NpcController>();
        if (npc != null)
        {
            if (npc.isCompanion)
            {
                turn.companionTurn++;
            }
            GetComponent<NpcMovement>().hasMoved = true;
        }
        return false;
    }

    //Add a function that does the overlapsphere (too much occurence)

    public bool CheckIfTagIsDifferent(string tagToCheck)
    {
        if(tagToCheck.ToLower() != tag.ToLower())
        {
            if (tagToCheck.ToLower() == "ground" || tagToCheck.ToLower() == "obstacle" || tagToCheck.ToLower() == "untagged") return false;
        }
        else
        {
            return false;
        }
        return true;
    }

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
