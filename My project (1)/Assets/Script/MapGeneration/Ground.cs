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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "characters")
        {
            IsEmpty = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "characters")
        {
            IsEmpty = true;
        }

    }
}
