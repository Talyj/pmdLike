using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //trois types de tours
    //0 = tour du joueur
    //1 = tour des compagnons
    //2 = tour des méchants

    public int companionTurn = 0;
    public int currentTurn = 0;
    public int totalTurn = 0;

    [SerializeField] private UI ui;

    public enum turn
    {
        playerTurn = 0,
        companionTurn = 1,
        enemyTurn = 2
    }

    public enum ModeDeCombat
    {
        classic = 0,
        tactique = 1
    }

    public GameObject player;
    public List<GameObject> companions = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    // 0 = Classique
    // 1 = tactique
    public int modeDeCombat = ((int)ModeDeCombat.classic);


    private GenerateCharacters charGen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charGen = GetComponent<GenerateCharacters>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnChange()
    {
        switch (currentTurn)
        {
            case ((int)turn.playerTurn):
                AddTurn();
                companionTurn = 0;
                if(companions.Count == 0)
                {
                    AddTurn();
                }
                break;
            case ((int)turn.companionTurn):
                CheckAllActed(companions);
                break;
            case ((int)turn.enemyTurn):
                CheckAllActed(enemies);
                break;
        }
        if (modeDeCombat == 1)
        {
            ui.GetNewPlayerController();
        }

    }
    private int CheckAllActed(List<GameObject> toCheck)
    {
        foreach(var npc in toCheck)
        {
            if (!npc.GetComponent<NpcMovement>().hasMoved) return 0;
        }
        AddTurn();
        if(enemies.Count <= 0)
        {
            AddTurn();
        }
        resetTurn(toCheck);
        return 1;
    }

    private void AddTurn()
    {
        currentTurn++;
        totalTurn++;
        if (currentTurn >= 3)
        {
            currentTurn = ((int)turn.playerTurn);
        }
        
    }
    public void ChangeMode()
    {
        modeDeCombat++;
        if (modeDeCombat >= 2)
        {
            modeDeCombat = 0;
        }
        Debug.Log("MODE CHANGE : " + modeDeCombat);
    }

    private void resetTurn(List<GameObject> toReset)
    {
        foreach(var npc in toReset)
        {
            npc.GetComponent<NpcMovement>().hasMoved = false;
        }
    }
}
