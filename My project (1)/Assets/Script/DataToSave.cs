using Assets.Script.Movements;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataToSave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReadData()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath+"/player.json");
        var data = JsonUtility.FromJson<Player>(jsonString);
    }

    public void SaveData(GameObject gameObjectToSave)
    {
        //CoMpanions to save
        List<Companion> companionsToSave = new List<Companion>();
        var companions = GameObject.FindGameObjectsWithTag("Player");

        foreach(var com in companions)
        {
            var companion = com.GetComponent<NpcController>();
            if(companion != null)
            {
                var comStats = com.GetComponent<Statistiques>();
                var comToSave = new Companion()
                {
                    job = com.GetComponent<Job>().job,
                    healthMax = comStats.healthMax,
                    manaMax = comStats.manaMax,
                    atq = comStats.atq,
                    def = comStats.def,
                };
                companionsToSave.Add(comToSave);
            }
        }

        //Save the player
        var stats = gameObjectToSave.GetComponent<Statistiques>();
        var playToSave = new Player()
        {
            job = gameObjectToSave.GetComponent<Job>().job,
            healthMax = stats.healthMax,
            manaMax = stats.manaMax,
            atq = stats.atq,
            def = stats.def,
            listCompanions = companionsToSave
        };

        string player = JsonUtility.ToJson(playToSave);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", player);
    }
}

public class Player
{
    public Job.Jobs job;
    public float healthMax;
    public float manaMax;
    public float atq;
    public float def;
    public List<Companion> listCompanions;
}

[Serializable]
public class Companion
{
    public Job.Jobs job;
    public float healthMax;
    public float manaMax;
    public float atq;
    public float def;
}
