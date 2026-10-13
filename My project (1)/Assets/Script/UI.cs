using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public PlayerController player;
    private TextMeshProUGUI textMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
    }

    public void DisplayHealth()
    {
         textMP.text = player.stats.healthCurrent.ToString() + "/" + player.stats.healthMax.ToString();
    }

    public void GetNewPlayerController()
    {
        //Change the player stats to display
    }
}
