using Assets.Script.Movements;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Statistiques player;
    [HideInInspector] public Job playerJob;
    [HideInInspector] public Statistiques mainPlayer;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Image backgroundSKills;
    [SerializeField] private List<Image> imageSkills;
    [SerializeField] private Dictionary<string, Sprite> listImg = new Dictionary<string, Sprite>();
    [SerializeField] public Dictionary<string ,List<string>> listSkill = new Dictionary<string, List<string>>();


    [HideInInspector] public TurnManager turn;
    [SerializeField] private TextMeshProUGUI TextForHealth;
    [SerializeField] private TextMeshProUGUI TextForModeDeCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turn = FindAnyObjectByType<TurnManager>();
        playerJob = player.GetComponent<Job>();
        StartCoroutine(DisplaySkills());
        if(turn != null)
        {
            DisplayModeDeCombat();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
    }

    public void DisplayModeDeCombat()
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
        playerJob = player.GetComponent<Job>();
        StartCoroutine(DisplaySkills());
    }

    public IEnumerator DisplaySkills()
    {
        while(playerJob == null)
        {
            yield return new WaitForEndOfFrame();
        }
        switch (playerJob.job)
        {
            case Job.Jobs.Tank:
                backgroundSKills.color = Color.yellow;
                break;
            case Job.Jobs.Dps:
                backgroundSKills.color = Color.red;
                break;
            case Job.Jobs.Healer:
                backgroundSKills.color = Color.green;
                break;
            case Job.Jobs.Troubadour:
                backgroundSKills.color = Color.blue;
                break;
        }
        for (int i = 0; i < imageSkills.Count; i++)
        {
            var test = listSkill[playerJob.job.ToString()];
            imageSkills[i].sprite = listImg[test[i]];
        }
    }

    
}
