using Assets.Script.Actions;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private NpcMovement npcMove;
    [SerializeField] private Statistiques damageable;
    [SerializeField] private NpcAction action;
    //MOdify mode to modify behavior of the companions(later)
    [SerializeField] private int mode = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        npcMove.FollowLeader();
        npcMove.EnemiesMove();
    }
}
