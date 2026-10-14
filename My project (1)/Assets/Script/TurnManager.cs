using NUnit.Framework;
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

    public GameObject player;
    public List<GameObject> companions = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    // 0 = Classique
    // 1 = tactique
    public int modeDeCombat = 0;


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
            case 0:
                AddTurn();
                companionTurn = 0;
                if(companions.Count == 0)
                {
                    AddTurn();
                }
                break;
            case 1:
                CheckAllActed(companions);
                break;
            case 2:
                CheckAllActed(enemies);
                break;
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
            currentTurn = 0;
        }
        
    }

    private void resetTurn(List<GameObject> toReset)
    {
        foreach(var npc in toReset)
        {
            npc.GetComponent<NpcMovement>().hasMoved = false;
        }
    }
}
