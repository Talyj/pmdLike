using Assets.Script.Actions;
using Assets.Script.Movements;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    //[SerializeField] private NpcMovement npcMove;
    [SerializeField] private Statistiques statistiques;
    [SerializeField] private NpcAction action;
    [SerializeField] private MovementGlobal Movement;
    [SerializeField] private PlayerAction playerActionMe;
    [HideInInspector] public PlayerController mainPlayer;

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
        mainPlayer = FindAnyObjectByType<PlayerController>();

        //turn = FindAnyObjectByType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCompanion && turn.currentTurn == myTurn && turn.companionTurn == orderId)
        {
            if(turn.modeDeCombat == ((int)TurnManager.ModeDeCombat.Classique))
            {
                //npcMove.CompanionBehavior();
                //ajouter la possibilité d'attaquer si à coté d'un ennemy
                //If there is an enemy around me i start trying to kill it instead of just following the player
                CompanionBehavior();

            }
            else if(turn.modeDeCombat == ((int)TurnManager.ModeDeCombat.Tactique) && turn.companionTurn == orderId)
            {
                if (!isControlled)
                {
                    isControlled = true;
                    Movement.target = transform.position;
                }
                Movement.MovementPlayer();
                job.BehaviorForJob();
            }
        }
        else if(!isCompanion && turn.currentTurn == myTurn)
        {
            //npcMove.EnemiesMove();
            EnemyBehavior();
            //turn.TurnChange();
        }
    }

    private void CompanionBehavior()
    {
        if (/*action.hasTarget && */action.target != null)
        {
            if (!Movement.isMoving && Vector3.Distance(action.target.transform.position, transform.position) <= /*action.atqRange*/5f)
            {
                //action.Attack();
                //hAve to decide which move to pick
                job.NpcBehaviorForJob();
            }
            else
            {
                Movement.Move(action.target.gameObject);
            }
        }
        else
        {
            Movement.Move(mainPlayer.gameObject);
        }
    }

    private void EnemyBehavior()
    {
        if (/*action.hasTarget && */action.target != null)
        {
            if (Vector3.Distance(action.target.transform.position, transform.position) <= action.atqRange && !Movement.isMoving)
            {
                action.Attack();
                //job.NpcBehaviorForJob();
            }
            else
            {
                Movement.Move(action.target.gameObject);
            }
        }
        else
        {
            Movement.Wait();
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
