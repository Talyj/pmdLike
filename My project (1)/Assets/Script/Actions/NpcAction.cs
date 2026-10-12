using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class NpcAction : MonoBehaviour
    {
        [SerializeField] private NpcMovement move;
        public PlayerController player;
        private float range = 5f;
        public bool hasTarget = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if(gameObject.tag == "enemy")
            //{
            //    CheckForTargets();
            //}
        }

        public bool CheckForTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
            foreach(var hit in hitColliders)
            {
                if(hit.tag.ToLower() == "player" /*|| hit.tag.ToLower() == "characters"*/)
                {
                    if(player == null)
                    {
                        player = hit.GetComponent<PlayerController>();
                    }
                    //Debug.Log("FOUUUUUUUUND");
                    hasTarget = true;
                    move.posGround = move.GetClosestTileToTarget(move.GetAroundTile()).transform.position;
                    return true;
                }
            }
            return false;   
        }

        public void ResetTargets()
        {
            player = null;
            hasTarget = false;
        }

        private void OnDrawGizmos()
        {
            if (gameObject.tag == "enemy")
            {
                Gizmos.color = Color.red;
                //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
                Gizmos.DrawWireSphere(transform.position, range);
                Gizmos.color = Color.yellow;
                //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
                Gizmos.DrawWireSphere(transform.position, 1.5f);
            }            
        }
    }
}