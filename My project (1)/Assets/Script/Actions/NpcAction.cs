using Assets.Script.Movements;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class NpcAction : MonoBehaviour
    {
        //[SerializeField] private NpcMovement move;
        [SerializeField] private MovementGlobal moveGlo;
        [SerializeField] private NpcController npcController;
        [HideInInspector]public Statistiques target;
        public float detectRange = 10f;
        public float atqRange = 5f;
        [HideInInspector] public bool hasTarget = false;


        public void Attack()
        {
            if (!moveGlo.hasMoved)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, atqRange);
                List<Statistiques> targets = new List<Statistiques>();
                foreach (var hit in hitColliders)
                {
                    if (hit.tag.ToLower() == "player" /*|| hit.tag.ToLower() == "characters"*/)
                    {
                        moveGlo.hasMoved = true;
                        //Attack....
                        transform.LookAt(target.transform.position);
                        targets.Add(hit.GetComponent<Statistiques>());
                        //hit.GetComponent<Statistiques>().TakeDamage(10);
                        //npcController.turn.TurnChange();
                    }
                }
                if (targets.Count > 0)
                {
                    moveGlo.hasMoved = true;
                    foreach (var tar in targets)
                    {
                        tar.GetComponent<Statistiques>().TakeDamage(10);
                    }
                    npcController.turn.TurnChange(gameObject);
                }
            }
        }

        public bool CheckForTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRange);
            var versus = "player";
            if (npcController.isCompanion)
            {
                versus = "enemy";
            }
            foreach(var hit in hitColliders)
            {
                //if (targets.Contains(hit.tag.ToLower()))
                if(hit.tag.ToLower() == versus)
                //if (hit.tag.ToLower()!= "ground" && tag.ToLower() != hit.tag.ToLower())
                {
                    if (target == null)
                    {
                        target = hit.GetComponent<Statistiques>();
                    }
                    //Debug.Log("FOUUUUUUUUND");
                    hasTarget = true;
                    //move.posGround = move.GetClosestTileToTarget(move.GetAroundTile(), target.transform.position, target).transform.position;
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

        public int GetBestAction(Job.Jobs job)
        {
            //switch (job)
            //{
            //    case Job.Jobs.Tank:
            //        break;
            //    case Job.Jobs.Dps:

            //        break;
            //    case Job.Jobs.Healer:
            //        break;
            //    case Job.Jobs.Troubadour:                    
            //        return UnityEngine.Random.Range(0, 3);
            //}
            //return 2;
            return UnityEngine.Random.Range(0, 3);
        }

        private void OnDrawGizmos()
        {
            if (gameObject.tag.ToLower() == "player")
            {
                Gizmos.color = Color.yellow;
                //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
                Gizmos.DrawWireSphere(transform.position, 5f);
            }
        }

    }
}