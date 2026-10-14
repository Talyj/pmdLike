using System;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [HideInInspector] public Statistiques player;
    [HideInInspector] public Statistiques mainPlayer;
    [SerializeField] private HealthBar healthBar;
    [HideInInspector] public TurnManager turn;
    private TextMeshProUGUI textMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMP = GetComponentInChildren<TextMeshProUGUI>();
        turn = FindAnyObjectByType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
    }

    public void DisplayHealth()
    {
        textMP.text = player.healthCurrent.ToString() + "/" + player.healthMax.ToString();
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
