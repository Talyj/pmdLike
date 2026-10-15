using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBarSprite;
    [HideInInspector] public Camera mainCamera;

    private void Awake()
    {
        mainCamera = FindAnyObjectByType<Camera>();
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
