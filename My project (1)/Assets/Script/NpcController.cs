using Assets.Script.Actions;
using Assets.Script.Movements;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private NpcMovement npcMove;
    [SerializeField] private Statistiques statistiques;
    [SerializeField] private NpcAction action;
    [SerializeField] private PlayerMovement playerMoveMe;
    [SerializeField] private PlayerAction playerActionMe;

    [HideInInspector] public Job job;
    [HideInInspector]public TurnManager turn;
    [HideInInspector] public bool isCompanion = true;
    [HideInInspector] public bool isControlled = false;
    [HideInInspector] public int orderId;
    [HideInInspector] public bool canAct = false;
    private int myTurn;



    //MOdify mode to modify behavior of the companions(later)
    //[SerializeField] private int mode = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTurn = GetComponent<Statistiques>().myTurn;
        job = GetComponent<Job>();
        //turn = FindAnyObjectByType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCompanion && turn.currentTurn == myTurn && turn.companionTurn == orderId)
        {
            if(turn.modeDeCombat == ((int)TurnManager.ModeDeCombat.Classique))
            {
                npcMove.FollowLeader();
                //ajouter la possibilité d'attaquer si à coté d'un ennemy
            }
            else if(turn.modeDeCombat == ((int)TurnManager.ModeDeCombat.Tactique) && turn.companionTurn == orderId)
            {
                if (!isControlled)
                {
                    isControlled = true;
                    playerMoveMe.target = transform.position;
                }
                playerMoveMe.Movement();
                job.BehaviorForJob();
            }
        }
        else if(!isCompanion && turn.currentTurn == myTurn)
        {
            npcMove.EnemiesMove();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (gameObject.tag == "enemy")
    //    {
    //        Gizmos.color = Color.red;
    //        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //        Gizmos.DrawWireSphere(transform.position, detectRange);
    //        Gizmos.color = Color.yellow;
    //        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //        Gizmos.DrawWireSphere(transform.position, atqRange);
    //    }            
    //}
}
