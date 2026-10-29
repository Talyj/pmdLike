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
using UnityEngine.Rendering;
using static TurnManager;

public class GenerateCharacters : MonoBehaviour
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject player;
    [HideInInspector] private GameObject mainPlayer;
    [SerializeField] private GameObject npc;
    [SerializeField] Camera camera;
    [SerializeField] Camera minimapCamera;
    [SerializeField] Camera mapCamera;
    [SerializeField] TurnManager turnManager;
    [SerializeField] private Material materialMechant;
    [SerializeField] private Dictionary<string, GameObject> allNpcAvailable = new Dictionary<string, GameObject>();
    [SerializeField] public List<string> allCompanions = new List<string>();
    [SerializeField] private Material materialCompanions;
    public List<Ground> grounds;
    private List<GameObject> cornerGrounds;
    public int numberOfEnemy = 0;

    [SerializeField] public GameObject goalToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounds = GetComponent<GenerateTerrain>().grounds;
        cornerGrounds = GetComponent<GenerateTerrain>().cornerGrounds;

        StartCoroutine(SpawnPlayer());        
        //SpawnCompanions(allCompanions);
        StartCoroutine(SpawnGoal());
    }

    // Update is called once per frame
    void Update()
    {
        BaseSpawnForDungeons(20);
    }

    public void BaseSpawnForDungeons(int numOfEnemies)
    {
        //Spawn them during the Player turn
        if (turnManager.currentTurn == 0 && numberOfEnemy < numOfEnemies)
        {
            SpawnEnemiesOnce(GetRandomNpc());
        }
    }

    private IEnumerator SpawnGoal()
    {
        while (grounds.Count() <= 0)
        {
            yield return new WaitForEndOfFrame();
        }
        var rdmChoice = UnityEngine.Random.Range(0, grounds.Count() - 1);
        while (Vector3.Distance(grounds[rdmChoice].transform.position, mainPlayer.transform.position) < 30)
        {
            rdmChoice = Random.Range(0, grounds.Count() - 1);
        }
        Vector3 goalPosition = new Vector3(grounds[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, grounds[rdmChoice].transform.position.z);
        //Vector3 goalPosition = new Vector3(0, turnManager.HeightMovementForEvryone, 8);
        GameObject goal = Instantiate(goalToSpawn, goalPosition, Quaternion.identity);

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
        var rdmChoice = 0;
        Vector3 rdmPos;
        while (!grounds[rdmChoice].IsEmpty)
        {
            rdmChoice = UnityEngine.Random.Range(0, grounds.Count() -1);
        }
        rdmPos = new Vector3(grounds[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, grounds[rdmChoice].transform.position.z);

        GameObject enemy = Instantiate(allNpcAvailable[npcName], rdmPos, Quaternion.identity);
        //GameObject enemy = Instantiate(npc, new Vector3(0, turnManager.HeightMovementForEvryone, 4), Quaternion.identity);
        enemy.GetComponent<NpcController>().isCompanion = false;
        enemy.GetComponent<NpcController>().turn = turnManager;
        //enemy.GetComponent<NpcMovement>().turn = turnManager;
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

    private IEnumerator SpawnPlayer()
    {
        while(grounds.Count() <= 0)
        {
            yield return new WaitForEndOfFrame();
        }

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
        var rdmChoice = UnityEngine.Random.Range(0, grounds.Count() - 1);
        var rdmPos = new Vector3(grounds[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, grounds[rdmChoice].transform.position.z);

        GameObject play = Instantiate(player, rdmPos, Quaternion.identity);
        //GameObject play = Instantiate(player, new Vector3(0, turnManager.HeightMovementForEvryone, 0), Quaternion.identity);

        //Variables to set
        play.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.playerTurn);
        play.GetComponent<PlayerController>().turn = turnManager;
        ui.player = play.GetComponent<Statistiques>();
        ui.playerJob = play.GetComponent<Job>();
        ui.mainPlayer = ui.player;
        mainPlayer = play;

        play.GetComponent<Statistiques>().healthMax = savedPlayer.healthMax;
        play.GetComponent<Statistiques>().manaMax = savedPlayer.manaMax;
        play.GetComponent<Statistiques>().atq = savedPlayer.atq;
        play.GetComponent<Statistiques>().def = savedPlayer.def;
        play.GetComponent<Job>().job = savedPlayer.job;
        //play.GetComponent<PlayerController>().allCompanions = savedPlayer.listCompanions;

        //Camera
        camera.GetComponent<PlayerFollow>().mainPlayer = play;
        camera.GetComponent<PlayerFollow>().targetPosition = play;
        //Minimap camera
        minimapCamera.GetComponent<PlayerFollow>().mainPlayer = play;
        minimapCamera.GetComponent<PlayerFollow>().targetPosition = play;

        //Move the Big map camera to the center
        var pos = cornerGrounds;
        var totX = 0f;
        var totZ = 0f;
        foreach(var corner in cornerGrounds)
        {
            totX += corner.transform.position.x;
            totZ += corner.transform.position.z;
        }
        var centerX = totX / cornerGrounds.Count();
        var centerZ = totZ / cornerGrounds.Count();
        mapCamera.transform.position = new Vector3(centerX, 88, centerZ);

        SpawnCompanions(allCompanions, play);

    }

    public void SpawnCompanions(List<string> allCompanions, GameObject player)
    {
        var rdmChoice = 0;
        Vector3 rdmPos;
        var tileAround = GetTilesAround(player.transform.position);

        //Add the spawn of the companion around the player
        for (int compOrder = 0; compOrder < allCompanions.Count; compOrder++)
        {
            //Companions
            while (!tileAround[rdmChoice].IsEmpty)
            {
                rdmChoice = UnityEngine.Random.Range(0, tileAround.Count());
            }
            tileAround.RemoveAt(rdmChoice);
            rdmPos = new Vector3(tileAround[rdmChoice].transform.position.x, turnManager.HeightMovementForEvryone, tileAround[rdmChoice].transform.position.z);


            GameObject companion = Instantiate(allNpcAvailable[allCompanions[compOrder]], rdmPos, Quaternion.identity);
            //GameObject companion = Instantiate(allNpcAvailable[allCompanions[compOrder]], new Vector3(1, turnManager.HeightMovementForEvryone, 0), Quaternion.identity);
            companion.tag = "Player";
            companion.GetComponent<NpcController>().isCompanion = true;
            //companion.GetComponent<NpcMovement>().targetStats = mainPlayer.GetComponent<Statistiques>();
            companion.GetComponent<NpcController>().turn = turnManager;
            //companion.GetComponent<NpcMovement>().turn = turnManager;
            companion.GetComponent<Statistiques>().myTurn = ((int)TurnManager.turn.companionTurn);
            //companion.GetComponent<Statistiques>().healthCurrent = 50;
            companion.GetComponent<NpcController>().orderId = compOrder;
            turnManager.companions.Add(companion);
            if (companion.GetComponent<MeshRenderer>() != null)
            { 
                companion.GetComponent<MeshRenderer>().material = materialCompanions;
            }
        }
    }

    public List<Ground> GetTilesAround(Vector3 targetPos)
    {
        List<Ground> res = new List<Ground>();

        //Get the collider around to see where i can go
        Collider[] hitColliders = Physics.OverlapSphere(targetPos, 2f);

        foreach (var col in hitColliders)
        {
            var direction = new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z) - transform.position;
            if (col.tag.ToLower() == "ground")
            {
                //Take only the tiles that are not me
                if (new Vector3(col.transform.position.x, turnManager.HeightMovementForEvryone, col.transform.position.z) != transform.position)
                {
                    //Debug.Log("my place");
                    res.Add(col.GetComponent<Ground>());
                }
            }
        }
        return res;
    }
}
