using Assets.Script.Actions;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private NpcMovement npcMove;
    [SerializeField] private Statistiques statistiques;
    [SerializeField] private NpcAction action;
    [SerializeField] private PlayerMovement playerMoveMe;
    [SerializeField] private PlayerAction playerActionMe;

    [HideInInspector]public TurnManager turn;
    [HideInInspector] public bool isCompanion = true;
    [HideInInspector] public bool isControlled = false;
    [HideInInspector] public int orderId;
    [HideInInspector] public bool canAct = false;
    private int myTurn;
    private UI ui;



    //MOdify mode to modify behavior of the companions(later)
    //[SerializeField] private int mode = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTurn = GetComponent<Statistiques>().myTurn;
        ui = FindAnyObjectByType<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCompanion && turn.currentTurn == myTurn && turn.companionTurn == orderId)
        {
            if(turn.modeDeCombat == 0)
            {
                npcMove.FollowLeader();
            }
            else if(turn.modeDeCombat == 1)
            {
                //TODO CAn put that in one if
                if(turn.companionTurn == orderId)
                {
                    //if (turn.modeDeCombat == 1)
                    //{
                    //    ui.GetNewPlayerController();
                    //}
                    //reset target de playermoveme une fois
                    if (!isControlled)
                    {
                        isControlled = true;
                        playerMoveMe.target = transform.position;
                    }
                    playerActionMe.Attack();
                    playerMoveMe.Movement();
                }
            }
        }
        else if(!isCompanion && turn.currentTurn == myTurn)
        {
            npcMove.EnemiesMove();
        }
    }
}
