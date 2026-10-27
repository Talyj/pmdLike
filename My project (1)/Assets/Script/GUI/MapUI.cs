using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private RawImage CanvaToActivate;
    [HideInInspector] public bool isActive = false;

    private void Update()
    {
        OpenCloseMap();
    }

    public void OpenCloseMap()
    {
        if (!isActive)
        {
            if (Input.GetButtonDown("Map"))
            {
                CanvaToActivate.gameObject.SetActive(true);
                isActive = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Map"))
            {
                CanvaToActivate.gameObject.SetActive(false);
                isActive = false;
            }
        }
    }
}
