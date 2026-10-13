using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerMovement move;
    [HideInInspector] public PlayerController playerCont;
    private int myTurn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTurn = GetComponent<Statistiques>().myTurn;
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();
    }

    public void Attack()
    {
        //If is moving cant input something else
        if (!move.isMoving)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move.isMoving = false;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
                foreach (var hit in hitColliders)
                {
                    if (hit.tag == "enemy")
                    {
                        var npc = GetComponent<NpcMovement>();
                        if (npc != null)
                        {
                            playerCont.turn.companionTurn++;
                            npc.hasActed = true;
                        }
                        hit.GetComponent<Statistiques>().TakeDamage(50);
                    }
                }
                playerCont.turn.TurnChange();
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
