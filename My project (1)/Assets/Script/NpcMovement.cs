using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class NpcMovement : MonoBehaviour
{
    private TurnManagement turn;

    [SerializeField] private float playerSpeed;
    public InputActionReference moveAction;


    //Player variables
    private PlayerController player;
    private Vector3 targetPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerController>();
        turn = Object.FindAnyObjectByType<TurnManagement>();
        targetPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(turn.currentTurn == 1)
        {
            FollowLeader();
        }
    }


    void FollowLeader()
    {
        //Base behavior for a companion
        //if it's my turn then i follow
        if(transform.position != targetPos)
        {
            //Before moving check the distance between me and the target, if it's too far change the targetPos to a closer tile
            if(Vector3.Distance(transform.position, targetPos) > 1.5)
            {
                //Get the collider around to see where i can go
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
                Collider closestCollider = new Collider();
                float distCollider = 2;

                //get direction to get to player
                Vector3 direction = targetPos - transform.position;

                //check every collider named "BodyPlayer" to not move if it's next to it
                foreach (var pos in hitColliders)
                {
                    if(pos.name == "BodyPlayer")
                    {
                        targetPos = transform.position;
                        break;
                    }
                    targetPos = transform.position + direction.normalized;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {              
            targetPos = player.transform.position;
            turn.TurnChange();
        }
    }

    void Attack()
    {
        //Add the behavior of a violent companion
    }

    void flee()
    {
        //Add the behavior of a wimpy companion
    }
}
