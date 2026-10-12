using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMove;
    [SerializeField] private Statistiques damageable;
    [SerializeField] private PlayerAction action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMove.Movement();
    }
}
