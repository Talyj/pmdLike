using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)] public float SmoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = PlayerTransform.position + _cameraOffset;
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
