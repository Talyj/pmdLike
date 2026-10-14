using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [HideInInspector] public GameObject mainPlayer;
    public GameObject targetPosition;
    public TurnManager turn;
    private Vector3 _cameraOffset;
    [Range(0.01f, 1.0f)] public float SmoothFactor = 0.5f;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    public void Start()
    {
        //PlayerTransform = FindAnyObjectByType<PlayerController>();
        //if(PlayerTransform == null)
        //{
        //    PlayerController.instanceStarted += OnPlayerInstanceStarted;
        //}
    }
    private void Update()
    {
    }

    public void LateUpdate()
    {
        if(turn.currentTurn == 0)
        {
            FollowTarget(mainPlayer.gameObject);
        }
        else if(turn.currentTurn == 1 && turn.modeDeCombat == 1)
        {
            FollowTarget(turn.companions[turn.companionTurn]);
        }
    }

    public void FollowTarget(GameObject target)
    {
        targetPosition = target;
        Vector3 newPos = target.transform.position + new Vector3(0, 3, -4);
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
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
