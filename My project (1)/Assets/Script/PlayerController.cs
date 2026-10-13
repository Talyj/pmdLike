using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerMovement playerMove;
    [SerializeField] public Statistiques damageable;
    [SerializeField] public PlayerAction action;
    public Statistiques stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<Statistiques>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.isDead)
        {
            playerMove.Movement();
            action.Attack();
        }
    }
}
