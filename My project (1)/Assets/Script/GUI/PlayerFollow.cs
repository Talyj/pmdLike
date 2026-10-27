using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [HideInInspector] public GameObject mainPlayer;
    public GameObject targetPosition;
    public TurnManager turn;
    public Vector3 _cameraOffset;
    [Range(0.01f, 1.0f)] public float SmoothFactor = 0.5f;
    private int position = 0;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    public void Start()
    {
        //_cameraOffset = new(0, 3, -4);
    }
    private void Update()
    {
        //MoveCameraAround();
    }

    public void LateUpdate()
    {
        if(mainPlayer != null)
        {
            ChangeCameraTarget();
        }
    }

    private void ChangeCameraTarget()
    {
        if (turn.currentTurn == ((int)TurnManager.turn.playerTurn))
        {
            FollowTarget(mainPlayer.gameObject);
        }
        else if (turn.currentTurn == ((int)TurnManager.turn.companionTurn) && turn.modeDeCombat == ((int)TurnManager.ModeDeCombat.Tactique))
        {
            FollowTarget(turn.companions[turn.companionTurn]);
        }
    }

    public void FollowTarget(GameObject target)
    {
        targetPosition = target;
        Vector3 newPos = target.transform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }

    public void MoveCameraAround()
    {
        if (Input.GetButtonUp("MoveCameraRight"))
        {
            //_cameraOffset = new Vector3(-4, 3, 0);
            //0 == base => 0, 3, -4
            position++;
            if(position > 3)
            {
                position = 0;
            }
            switch (position)
            {
                //Check why the rotation breaks everything
                case 0:
                    _cameraOffset = new Vector3(0, 3, -4);
                    transform.Rotate(new Vector3(0, -90, 0), Space.World);
                    break;
                case 1:
                    _cameraOffset = new Vector3(4, 3, 0);
                    transform.Rotate(new Vector3(0, -90, 0), Space.World);
                    break;
                case 2:
                    _cameraOffset = new Vector3(0, 3, 4);
                    transform.Rotate(new Vector3(0, -90, 0), Space.World);
                    break;
                case 3:
                    _cameraOffset = new Vector3(-4, 3, 0);
                    transform.Rotate(new Vector3(0, -90, 0), Space.World);
                    break;
            }
        }
        if (Input.GetButtonUp("MoveCameraLeft"))
        {
            //0 == base => 0, 3, -4
            position--;
            if (position < 0)
            {
                position = 3;
            }
            switch (position)
            {
                case 0:
                    _cameraOffset = new Vector3(0, 3, -4);
                    transform.Rotate(new Vector3(0, 90, 0), Space.World);
                    break;
                case 1:
                    _cameraOffset = new Vector3(4, 3, 0);
                    transform.Rotate(new Vector3(0, 90, 0), Space.World);
                    break;
                case 2:
                    _cameraOffset = new Vector3(0, 3, 4);
                    transform.Rotate(new Vector3(0, 90, 0), Space.World);
                    break;
                case 3:
                    _cameraOffset = new Vector3(-4, 3, 0);
                    transform.Rotate(new Vector3(0, 90, 0), Space.World);
                    break;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("BloquePorte"))
        {
            MeshRenderer temp = other.gameObject.GetComponent<MeshRenderer>();
            temp.enabled = false;
        }
    }
}
