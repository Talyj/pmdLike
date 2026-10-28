using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //Variables for everyone
    [HideInInspector] public float HeightMovementForEvryone = .35f;
    [HideInInspector] public float RangeViewNotClip = 1.5f;


    //trois types de tours
    //0 = tour du joueur
    //1 = tour des compagnons
    //2 = tour des méchants
    public int companionTurn = 0;
    public int currentTurn = 0;
    public int totalTurn = 0;

    [SerializeField] private UI ui;
    [SerializeField] private DamageOverTime dot;

    public enum turn
    {
        playerTurn = 0,
        companionTurn = 1,
        enemyTurn = 2
    }

    public enum ModeDeCombat
    {
        Classique = 0,
        Tactique = 1
    }

    //public GameObject player;
    public List<GameObject> companions = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    // 0 = Classique
    // 1 = tactique
    public int modeDeCombat = ((int)ModeDeCombat.Classique);


    private GenerateCharacters charGen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charGen = GetComponent<GenerateCharacters>();
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
                    if (enemies.Count == 0)
                    {
                        AddTurn();
                    }
                }
                break;
            case ((int)turn.companionTurn):
                CheckAllActed(companions);
                break;
            case ((int)turn.enemyTurn):
                CheckAllActed(enemies);
                //AddTurn();
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

        if(enemies.Count <= 0)
        {
            AddTurn();
        }
        AddTurn();
        resetTurn(toCheck);
        return 1;
    }

    private void AddTurn()
    {
        dot.DealDot();
        //dot.listCharacterToDebuff = dot.DealDot();

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
        ui.DisplayModeDeCombat();
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
