using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool IsEmpty = true;

    // Update is called once per frame
    void Update()
    {
        CheckIfEmpty();
    }


    private void CheckIfEmpty()
    {
        if (Physics.Raycast(transform.position, Vector3.up, 1.0f))
        {
            IsEmpty = false;
        }
        else
        {
            IsEmpty = true;
        }
    }
}
