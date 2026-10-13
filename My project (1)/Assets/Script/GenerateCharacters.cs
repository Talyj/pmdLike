using Assets.Script.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class GenerateCharacters : MonoBehaviour
{

    public float heightSpawn = 3.5f;

    [SerializeField] private UI ui;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] Camera camera;
    [SerializeField] TurnManager turnManager;
    [SerializeField] private Material materialMechant;
    [SerializeField] private Material materialCompanions;
    public List<Ground> grounds;
    public int numberOfEnemy = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounds = GetComponent<GenerateTerrain>().grounds;
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnManager.totalTurn == 0 && numberOfEnemy < 10)
        {
            SpawnEnemiesOnce();
        }
    }

    public IEnumerator SpawnEnemiesLoop()
    {
        while (true)
        {
            if (grounds.Count() > 0 && numberOfEnemy < 10)
            {
                SpawnEnemiesOnce();
            }
            yield return new WaitForSeconds(10);
        }
    }

    public void SpawnEnemiesOnce()
    {
        var rdmChoice = UnityEngine.Random.Range(0, grounds.Count());

        GameObject enemy = Instantiate(npc, new Vector3(grounds[rdmChoice].transform.position.x, heightSpawn, grounds[rdmChoice].transform.position.z), Quaternion.identity);
        //GameObject enemy = Instantiate(npc, new Vector3(0, 3.5f, 5), Quaternion.identity);
        enemy.GetComponent<NpcController>().isCompanion = false;
        enemy.GetComponent<NpcController>().turn = turnManager;
        enemy.GetComponent<MeshRenderer>().material = materialMechant;
        enemy.GetComponent<Statistiques>().myTurn = 2;
        enemy.tag = "enemy";
        turnManager.enemies.Add(enemy);
        numberOfEnemy++;
    }

    void SpawnPlayer()
    {
        //Player
        // TODO Give all the variables to the controller
        GameObject play = Instantiate(player, new Vector3(0, 3.5f, 0), Quaternion.identity);
        //GameObject play = Instantiate(player, new Vector3(5, 3.5f, -5), Quaternion.identity);
        play.GetComponent<PlayerAction>().playerCont = play.GetComponent<PlayerController>();
        play.GetComponent<PlayerController>().turn = turnManager;
        play.GetComponent<Statistiques>().myTurn = 0;
        turnManager.player = play;
        ui.player = play.GetComponent<Statistiques>();
        ui.mainPlayer = play.GetComponent<Statistiques>();
        //Camera
        camera.GetComponent<PlayerFollow>().mainPlayer = play;
        camera.GetComponent<PlayerFollow>().turn = turnManager;
        camera.GetComponent<PlayerFollow>().targetPosition = play;        


        //Companions
        // TODO Give all the variables to the controller
        GameObject companion = Instantiate(npc, new Vector3(1, heightSpawn, 0), Quaternion.identity);
        companion.GetComponent<MeshRenderer>().material = materialCompanions;
        companion.GetComponent<NpcMovement>().player = play.GetComponent<PlayerMovement>();
        companion.GetComponent<NpcController>().turn = turnManager;
        companion.GetComponent<Statistiques>().myTurn = 1;
        companion.GetComponent<NpcController>().orderId = 0;
        companion.GetComponent<PlayerAction>().playerCont = play.GetComponent<PlayerController>();
        turnManager.companions.Add(companion);

        //test second companions
        //GameObject companion1 = Instantiate(npc, new Vector3(1, heightSpawn, 1), Quaternion.identity);
        //companion1.GetComponent<MeshRenderer>().material = materialCompanions;
        //companion1.GetComponent<NpcMovement>().player = play.GetComponent<PlayerMovement>();
        //companion1.GetComponent<NpcController>().turn = turnManager;
        //companion1.GetComponent<Statistiques>().myTurn = 1;
        //companion1.GetComponent<NpcController>().orderId = 1;
        //turnManager.companions.Add(companion1);
    }
}
