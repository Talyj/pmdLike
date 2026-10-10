using System.Collections;
using UnityEngine;

public class TurnManagement : MonoBehaviour
{
    //trois types de tours
    //0 = tour du joueur
    //1 = tour des compagnons
    //2 = tour des méchants

    public int currentTurn = 0;

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
        currentTurn++;
        if(currentTurn >= 2)
        {
            currentTurn = 0;
        }
    }
}
