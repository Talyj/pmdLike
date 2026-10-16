using System;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [HideInInspector] public Statistiques player;
    [HideInInspector] public Statistiques mainPlayer;
    [SerializeField] private HealthBar healthBar;
    [HideInInspector] public TurnManager turn;
    [SerializeField] private TextMeshProUGUI TextForHealth;
    [SerializeField] private TextMeshProUGUI TextForModeDeCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turn = FindAnyObjectByType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
        DisplayModeDeCombat();
    }

    private void DisplayModeDeCombat()
    {
        TextForModeDeCombat.text = System.Enum.GetName(typeof(TurnManager.ModeDeCombat), turn.modeDeCombat) ;
    }

    private void DisplayHealth()
    {
        TextForHealth.text = player.healthCurrent.ToString() + "/" + player.healthMax.ToString();
        healthBar.UpdateHealthBar(player.healthCurrent, player.healthMax);
    }

    public void GetNewPlayerController()
    {
        //Change the player stats to display
        if(turn.companions.Count <= turn.companionTurn)
        {
            player = mainPlayer;
        }
        else
        {
            player = turn.companions[turn.companionTurn].GetComponent<Statistiques>();
        }
    }
}
