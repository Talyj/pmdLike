using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerMovement playerMove;
    [SerializeField] public Statistiques damageable;
    [SerializeField] public PlayerAction action;
    public TurnManager turn;
    public Statistiques stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<Statistiques>();
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
                ChangeMode();
            }
            playerMove.Movement();
            action.Attack();
        }
    }

    private void ChangeMode()
    {
        turn.modeDeCombat++;
        if(turn.modeDeCombat >= 2)
        {
            turn.modeDeCombat = 0;
        }
        Debug.Log("MODE CHANGE : " + turn.modeDeCombat);
    }
}
