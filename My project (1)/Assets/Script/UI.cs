using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Statistiques player;
    public Statistiques mainPlayer;
    private TextMeshProUGUI textMP;
    public TurnManager turn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();
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
    }

    public void GetNewPlayerController()
    {
        //Change the player stats to display
        if(turn.modeDeCombat == 1 && turn.currentTurn == 1)
        {
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
}
