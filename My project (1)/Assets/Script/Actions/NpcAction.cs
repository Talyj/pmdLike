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
        [SerializeField] private NpcMovement move;
        [SerializeField] private NpcController npcController;
        [HideInInspector]public Statistiques target;
        [HideInInspector] public bool hasTarget = false;
        [HideInInspector] private Statistiques myStats;

        private void Start()
        {
            myStats = GetComponent<Statistiques>();
        }


        public void Attack()
        {
            if (!move.hasMoved)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, myStats.atqRange);
                List<Statistiques> targets = new List<Statistiques>();
                foreach (var hit in hitColliders)
                {
                    if (hit.tag.ToLower() == "player" /*|| hit.tag.ToLower() == "characters"*/)
                    {
                        move.hasMoved = true;
                        //Attack....
                        transform.LookAt(target.transform.position);
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

        public bool CheckForTargets(List<string> targets)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, myStats.detectRange);
            RaycastHit ray;
            foreach(var hit in hitColliders)
            {
                if (targets.Contains(hit.tag.ToLower()))
                {
                    var direction = hit.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, direction, out ray, myStats.detectRange))
                    {
                        if (targets.Contains(ray.collider.tag.ToLower()))
                        {
                            if (target == null)
                            {
                                target = hit.GetComponent<Statistiques>();
                            }
                            //Debug.Log("FOUUUUUUUUND");
                            hasTarget = true;
                            move.posGround = move.GetClosestTileToTarget(move.GetAroundTile(), target.transform.position, target).transform.position;
                            return true;
                        }
                    }
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
            Debug.Log("test");
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
            if (gameObject.tag.ToLower() == "enemy")
            {
                Gizmos.color = Color.yellow;
                //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
                Gizmos.DrawWireSphere(transform.position, 5f);
            }
        }

    }
}