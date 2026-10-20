using Assets.Script.Actions;
using Assets.Script.Movements;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using TMPro;
using UnityEngine;
using static TurnManager;

public class GenerateCharacters : MonoBehaviour
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject player;
    [HideInInspector] private GameObject mainPlayer;
    [SerializeField] private GameObject npc;
    [SerializeField] Camera camera;
    [SerializeField] TurnManager turnManager;
    [SerializeField] private Material materialMechant;
    [SerializeField] private Dictionary<string, GameObject> allNpcAvailable = new Dictionary<string, GameObject>();
    [SerializeField] public List<string> allCompanions = new List<string>();
    [SerializeField] private Material materialCompanions;
    public List<Ground> grounds;
    public int numberOfEnemy = 0;

    [SerializeField] public GameObject goalToSpawn;
    private bool isGoalThere = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounds = GetComponent<GenerateTerrain>().grounds;
        SpawnPlayer();
        SpawnCompanions(allCompanions);
    }

    // Update is called once per frame
    void Update()
    {
        BaseSpawnForDungeons(20);
        SpawnGoal();
    }

    public void BaseSpawnForDungeons(int numOfEnemies)
    {
        //Spawn them during the Player turn
        if (turnManager.totalTurn == 0 && numberOfEnemy < numOfEnemies)
        {
            SpawnEnemiesOnce(GetRandomNpc());
        }
    }

    public IEnumerator SpawnEnemiesLoop(int numOfEnemies)
    {
        while (true)
        {
            if (grounds.Count() > 0 && numberOfEnemy < numOfEnemies)
            {

                SpawnEnemiesOnce(GetRandomNpc());
            }
            yield return new WaitForSeconds(10);
        }
    }

    public void SpawnGoal()
    {
        if (!isGoalThere)
        {
            var rdmChoice = UnityEngine.Random.Range(0, grounds.Count() - 1);
            while (Vector3.Distance(grounds[rdmChoice].transform.position, mainPlayer.transform.position) < 30)
            {
                rdmChoice = Random.Range(0, grounds.Count() - 1);
            }
            //Vector3 goalPosition = new Vector3(grounds[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, grounds[rdmChoice].transform.position.z);
            Vector3 goalPosition = new Vector3(0 , turnManager.HeightMovementForEvryone, 8);
            GameObject goal = Instantiate(goalToSpawn, goalPosition, Quaternion.identity);
            isGoalThere = true;
        }
    }

    public string GetRandomNpc()
    {
        var rdm = Random.Range(0, allNpcAvailable.Count);
        var nameList = new List<string>();
        foreach(var name in allNpcAvailable)
        {
            nameList.Add(name.Key);
        }

        return nameList[rdm];
    }

    public void SpawnEnemiesOnce(string npcName)
    {
        var rdmChoice = UnityEngine.Random.Range(0, grounds.Count() -1);

        GameObject enemy = Instantiate(allNpcAvailable[npcName], new Vector3(grounds[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, grounds[rdmChoice].transform.position.z), Quaternion.identity);
        //GameObject enemy = Instantiate(npc, new Vector3(0, turnManager.HeightMovementForEvryone, 4), Quaternion.identity);
        enemy.GetComponent<NpcController>().isCompanion = false;
        enemy.GetComponent<NpcController>().turn = turnManager;
        enemy.GetComponent<NpcMovement>().turn = turnManager;
        enemy.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.enemyTurn);
        enemy.tag = "enemy";
        turnManager.enemies.Add(enemy);
        numberOfEnemy++;
        //This should be the only parameters that will change when i have models
        if(enemy.GetComponent<MeshRenderer>() != null)
        {
            enemy.GetComponent<MeshRenderer>().material = materialMechant;
        }
    }

    void SpawnPlayer()
    {
        var fileName = Application.persistentDataPath + "/save.json";
        Player savedPlayer;
        if (File.Exists(fileName))
        {
            string jsonString = File.ReadAllText(Application.persistentDataPath + "/save.json");
            savedPlayer = JsonUtility.FromJson<Player>(jsonString);
        }
        else
        {
            //use the default value
            string jsonString = File.ReadAllText(Application.persistentDataPath + "/default.json");
            savedPlayer = JsonUtility.FromJson<Player>(jsonString);
        }

        //Player
        // TODO Give all the variables to the controller
        GameObject play = Instantiate(player, new Vector3(0, turnManager.HeightMovementForEvryone, 0), Quaternion.identity);
        //GameObject play = Instantiate(player, new Vector3(5, turnManager.HeightMovementForEvryone, -5), Quaternion.identity);

        play.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.playerTurn);
        play.GetComponent<PlayerController>().turn = turnManager;
        ui.playerJob = play.GetComponent<Job>();
        ui.player = play.GetComponent<Statistiques>();
        ui.mainPlayer = ui.player;
        mainPlayer = play;

        play.GetComponent<Statistiques>().healthMax = 500;
        play.GetComponent<Statistiques>().manaMax = 500;
        play.GetComponent<Statistiques>().atq = 50;
        play.GetComponent<Statistiques>().def = 0;
        //play.GetComponent<Job>().job = savedPlayer.job;

        //Camera
        camera.GetComponent<PlayerFollow>().mainPlayer = play;
        camera.GetComponent<PlayerFollow>().targetPosition = play;        
    }

    public void SpawnCompanions(List<string> allCompanions)
    {
        //Add the spawn of the companion around the player
        for(int compOrder = 0; compOrder < allCompanions.Count; compOrder++)
        {
            //mainPlayer.GetComponent<PlayerMovement>().
            //Companions
            GameObject companion = Instantiate(allNpcAvailable[allCompanions[compOrder]], new Vector3(1, turnManager.HeightMovementForEvryone, 0), Quaternion.identity);
            companion.tag = "characters";
            companion.GetComponent<NpcController>().isCompanion = true;
            companion.GetComponent<NpcMovement>().targetStats = mainPlayer.GetComponent<Statistiques>();
            companion.GetComponent<NpcController>().turn = turnManager;
            companion.GetComponent<NpcMovement>().turn = turnManager;
            companion.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.companionTurn);
            //companion.GetComponent<Statistiques>().healthCurrent = 50;
            companion.GetComponent<NpcController>().orderId = compOrder;
            turnManager.companions.Add(companion);
            if (companion.GetComponent<MeshRenderer>() != null)
            { 
                companion.GetComponent<MeshRenderer>().material = materialCompanions;
            }
        }



        ////test second companions
        //GameObject companion1 = Instantiate(npc, new Vector3(1, turnManager.HeightMovementForEvryone, 1), Quaternion.identity);
        //companion1.GetComponent<MeshRenderer>().material = materialCompanions;
        //companion1.GetComponent<NpcMovement>().player = mainPlayer.GetComponent<PlayerController>();
        //companion1.GetComponent<NpcController>().turn = turnManager;
        //companion1.GetComponent<Statistiques>().myTurn = 1;
        //companion.GetComponent<Statistiques>().healthCurrent = 45;
        //companion1.GetComponent<NpcController>().orderId = 1;
        //turnManager.companions.Add(companion1);
    }
}
