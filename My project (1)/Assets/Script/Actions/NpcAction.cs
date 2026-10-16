using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class NpcAction : MonoBehaviour
    {
        [SerializeField] private NpcMovement move;
        [SerializeField] private NpcController npcController;
        [HideInInspector]public Statistiques target;
        private float detectRange = 5f;
        public float atqRange = 2f;
        [HideInInspector] public bool hasTarget = false;


        public void Attack()
        {
            if (!move.hasMoved)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, atqRange);
                List<Statistiques> targets = new List<Statistiques>();
                foreach (var hit in hitColliders)
                {
                    if (hit.tag.ToLower() == "player" || hit.tag.ToLower() == "characters")
                    {
                        move.hasMoved = true;
                        //Attack....
                        targets.Add(hit.GetComponent<Statistiques>());
                        //hit.GetComponent<Statistiques>().TakeDamage(10);
                        //npcController.turn.TurnChange();
                    }
                }
                if (targets.Count > 0)
                {
                    foreach (var tar in targets)
                    {
                        tar.GetComponent<Statistiques>().TakeDamage(10);
                    }
                    npcController.turn.TurnChange();
                }
            }
        }

        public bool CheckForTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRange);
            foreach(var hit in hitColliders)
            {
                if(hit.tag.ToLower() == "player" || hit.tag.ToLower() == "characters")
                {
                    if(target == null)
                    {
                        target = hit.GetComponent<Statistiques>();
                    }
                    //Debug.Log("FOUUUUUUUUND");
                    hasTarget = true;
                    move.posGround = move.GetClosestTileToTarget(move.GetAroundTile(), target.transform.position, target).transform.position;
                    return true;
                }
            }
            return false;   
        }

        public void ResetTargets()
        {
            target = null;
            hasTarget = false;
        }

    }
}