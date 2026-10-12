using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAction : MonoBehaviour
{
    public PlayerMovement player;
    public TurnManagement turn;
    [SerializeField] private PlayerMovement move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();
    }

    public void Attack()
    {

        //0 = player turn
        if (turn.currentTurn == 0)
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
                            //hit.GetComponent<Health>().Dies();
                            hit.GetComponent<Statistiques>().TakeDamage(50);
                        }
                    }
                    turn.TurnChange();
                }

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
