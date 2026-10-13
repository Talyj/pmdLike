using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool IsEmpty = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
