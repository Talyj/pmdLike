using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool IsEmpty = true;
    RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        CheckIfEmpty();
    }

    private void CheckIfEmpty()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f))
        {
            IsEmpty = false;
        }
        else
        {
            IsEmpty = true;
        }
        Debug.DrawRay(transform.position, Vector3.up, Color.yellow);
    }
}
