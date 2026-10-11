using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagement : MonoBehaviour
{
    //trois types de tours
    //0 = tour du joueur
    //1 = tour des compagnons
    //2 = tour des méchants

    public int currentTurn = 0;

    public GameObject player;
    public List<GameObject> companions = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            if (!npc.GetComponent<NpcMovement>().hasActed) return 0;
        }
        AddTurn();
        resetTurn(toCheck);
        return 1;
    }

    private void AddTurn()
    {
        currentTurn++;
        if (currentTurn >= 3)
        {
            currentTurn = 0;
        }
        
    }

    private void resetTurn(List<GameObject> toReset)
    {
        foreach(var npc in toReset)
        {
            npc.GetComponent<NpcMovement>().hasActed = false;
        }
    }
}
