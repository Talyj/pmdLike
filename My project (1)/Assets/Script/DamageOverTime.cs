using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    [NonSerialized]public Dictionary<GameObject, int> listCharacterToDebuff = new Dictionary<GameObject, int>();
    [SerializeField] private TurnManager turn;

    //public Dictionary<GameObject, int> DealDot()
    public void DealDot()
    {
        Dictionary<GameObject, int> res = new Dictionary<GameObject, int>();
        var listToModify = new List<GameObject>();

        //Deal the damage % of max hp

            //Check the list
        foreach (var pla in listCharacterToDebuff)
        {
            if (pla.Key.GetComponent<Statistiques>().myTurn == turn.currentTurn)
            {

                pla.Key.GetComponent<Statistiques>().TakeDamage((float)(pla.Key.GetComponent<Statistiques>().healthMax * 0.1));
                listToModify.Add(pla.Key);
            }
        }
        foreach(var mod in listToModify)
        {
            listCharacterToDebuff[mod] -= 1;
            if (listCharacterToDebuff[mod] <= 0)
            {
                listCharacterToDebuff.Remove(mod);
            }
        }

    }
}
