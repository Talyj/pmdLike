using Assets.Script.Actions;
using Assets.Script.Movements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    [SerializeField] private List<string> allCompanions = new List<string>();
    [SerializeField] private Material materialCompanions;
    public List<Ground> grounds;
    public int numberOfEnemy = 0;

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
        BaseSpawnForDungeons(1);
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

        //GameObject enemy = Instantiate(allNpcAvailable[npcName], new Vector3(grounds[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, grounds[rdmChoice].transform.position.z), Quaternion.identity);
        GameObject enemy = Instantiate(npc, new Vector3(0, turnManager.HeightMovementForEvryone, 4), Quaternion.identity);
        enemy.GetComponent<NpcController>().isCompanion = false;
        enemy.GetComponent<NpcController>().turn = turnManager;
        enemy.GetComponent<NpcMovement>().turn = turnManager;
        enemy.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.enemyTurn);
        enemy.tag = "enemy";
        turnManager.enemies.Add(enemy);
        numberOfEnemy++;
        //This should be the only parameters that will change when i have models
        enemy.GetComponent<MeshRenderer>().material = materialMechant;
    }

    void SpawnPlayer()
    {
        //Player
        // TODO Give all the variables to the controller
        GameObject play = Instantiate(player, new Vector3(0, turnManager.HeightMovementForEvryone, 0), Quaternion.identity);
        //GameObject play = Instantiate(player, new Vector3(5, turnManager.HeightMovementForEvryone, -5), Quaternion.identity);
        play.GetComponent<PlayerController>().turn = turnManager;
        play.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.playerTurn);
        play.GetComponent<Statistiques>().healthMax = 1000;
        play.GetComponent<Statistiques>().manaMax = 1000;
        ui.player = play.GetComponent<Statistiques>();
        ui.playerJob = play.GetComponent<Job>();
        //turnManager.player = play;
        ui.mainPlayer = ui.player;
        mainPlayer = play;
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
            companion.GetComponent<MeshRenderer>().material = materialCompanions;
            companion.GetComponent<NpcMovement>().targetStats = mainPlayer.GetComponent<Statistiques>();
            companion.GetComponent<NpcController>().turn = turnManager;
            companion.GetComponent<NpcMovement>().turn = turnManager;
            companion.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.companionTurn);
            //companion.GetComponent<Statistiques>().healthCurrent = 50;
            companion.GetComponent<NpcController>().orderId = compOrder;
            turnManager.companions.Add(companion);
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
