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
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    CreateDefaultValues();
        //}
    }
    public DefaultValuesToLoad ReadDefaultSave()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath+"/default.json");
        return JsonUtility.FromJson<DefaultValuesToLoad>(jsonString);
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

    public void CreateDefaultValues()
    {
        DefaultValuesToLoad valToSave = new DefaultValuesToLoad();
        List<DefaultJobStats> jobs = new List<DefaultJobStats>();
        var tank = new DefaultJobStats()
        {
            jobName = Job.Jobs.Tank,
            healthMax = 500,
            manaMax = 100,
            atq = 40,
            def = 50,
            detectRange = 4,
            atqRange = 3
        };
        var dps = new DefaultJobStats()
        {
            jobName = Job.Jobs.Dps,
            healthMax = 200,
            manaMax = 200,
            atq = 60,
            def = 20,
            detectRange = 5,
            atqRange = 3
        };
        var heal = new DefaultJobStats()
        {
            jobName = Job.Jobs.Healer,
            healthMax = 150,
            manaMax = 300,
            atq = 20,
            def = 20,
            detectRange = 6,
            atqRange = 5
        };
        var troubadour = new DefaultJobStats()
        {
            jobName = Job.Jobs.Troubadour,
            healthMax = 300,
            manaMax = 200,
            atq = 40,
            def = 40,
            detectRange = 5,
            atqRange = 5
        };

        jobs.Add(troubadour);
        jobs.Add(tank);
        jobs.Add(dps);
        jobs.Add(heal);
        valToSave.jobs = jobs;
        string player = JsonUtility.ToJson(valToSave);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/default.json", player);
    }
}

public class DefaultValuesToLoad
{
    public List<DefaultJobStats> jobs;
}

[Serializable]
public class DefaultJobStats
{
    public Job.Jobs jobName;
    public float healthMax;
    public float manaMax;
    public float atq;
    public float def;
    public float detectRange;
    public float atqRange;
}

public class Player
{
    public Job.Jobs job;
    public float healthMax;
    public float manaMax;
    public float atq;
    public float def;
    public Player otherJobsList;
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
