using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VillageMovement : MonoBehaviour
{
    private float playerSpeed = 10.0f;

    public InputActionReference moveAction;
    public CharacterController controller;

    private void OnEnable()
    {
        moveAction.action.Enable();
        GetComponent<Statistiques>().GetStats();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Statistiques>().GetStats();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
            transform.forward = move;


        Vector3 finalMove = move * playerSpeed;
        controller.Move(finalMove * Time.deltaTime);
    }
}
