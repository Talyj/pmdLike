using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBarSprite;
    [HideInInspector] public Camera mainCamera;

    private void Awake()
    {
        
        var cameras = FindObjectsByType<Camera>();
        foreach(var cam in cameras)
        {
            if(cam.name.ToLower().Contains("main"))
            {
                mainCamera = cam;
                break;
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
    
    public void LookAtCamera()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
