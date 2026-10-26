using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenUI : MonoBehaviour
{
    [SerializeField] private Canvas CanvaToActivate;
    private bool isActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfPlayerIsAround())
        {
            SetCanva();
        }
    }

    public bool CheckIfPlayerIsAround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var col in hitColliders)
        {
            if (col.tag.ToLower() == "player")
            {
                return true;               
            }
        }
        CanvaToActivate.gameObject.SetActive(false);
        isActive = false;
        return false;
    }

    public void SetCanva()
    {
        if(!isActive)
        {
            if (Input.GetButtonDown("Jump"))
            {
                CanvaToActivate.gameObject.SetActive(true);
                isActive = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                CanvaToActivate.gameObject.SetActive(false);
                isActive = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, 5f);        
    }
}
