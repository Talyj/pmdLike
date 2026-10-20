using Assets.Script.Movements;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerMovement playerMove;
    [SerializeField] public Statistiques damageable;
    [SerializeField] public PlayerAction action;
    [HideInInspector] public Job job;
    public TurnManager turn;
    public Statistiques stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<Statistiques>();
        job = GetComponent<Job>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.isDead && turn.currentTurn == stats.myTurn)
        {
            //LCTRL
            //if (Input.GetButtonDown("Fire1"))
            if (Input.GetKeyDown(KeyCode.J))
            {
                turn.ChangeMode();
            }

            playerMove.Movement();
            job.BehaviorForJob();

            if (Input.GetKeyUp(KeyCode.L))
            {
                //action.SimpleAttack(5f, 25);
                //StartCoroutine(action.Grab(5f, 5));
                //action.Heal(2f, 25);
                //action.HealAround(3f, 10);
                //CHeckGrapple
                StartCoroutine(action.Grapple(5f, 5));                
            }
            ////Why the fuck did i need the second check ?? (in case the turn of some companions get jumped after attacking just add this condition)
            //if (Input.GetButtonUp("Jump")/* && mainCamera.targetPosition == gameObject*/)
            //{
            //    action.Attack();
            //}
            //Add a Job class to make them do the differents attacks
            // this function will call the jobBehavior with all the attacks
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //    Gizmos.DrawWireSphere(transform.position, 2f);
    //}
}
