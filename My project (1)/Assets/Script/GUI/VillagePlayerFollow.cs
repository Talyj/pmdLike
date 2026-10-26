using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillagePlayerFollow : MonoBehaviour
{

    public GameObject targetPosition;
    private Vector3 _cameraOffset;
    [Range(0.01f, 1.0f)] public float SmoothFactor = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraOffset = new(0, 3, -4);
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        if(targetPosition != null)
        {
            Vector3 newPos = targetPosition.transform.position + _cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
        }
        else
        {
            //Moche mais pour l'instant ça marche
            SceneManager.LoadScene("Village");
        }
    }
}
