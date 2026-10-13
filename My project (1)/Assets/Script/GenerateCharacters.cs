using Assets.Script.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class GenerateCharacters : MonoBehaviour
{

    public float heightSpawn = 3.5f;

    [SerializeField] private UI ui;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] Camera camera;
    [SerializeField] TurnManagement turnManager;
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
        enemy.GetComponent<NpcMovement>().isCompanion = false;
        enemy.GetComponent<MeshRenderer>().material = materialMechant;
        enemy.GetComponent<NpcAction>().turn = turnManager;
        enemy.tag = "enemy";
        turnManager.enemies.Add(enemy);
        numberOfEnemy++;
    }

    void SpawnPlayer()
    {
        //Player
        GameObject play = Instantiate(player, new Vector3(0, 3.5f, 0), Quaternion.identity);
        //GameObject play = Instantiate(player, new Vector3(5, 3.5f, -5), Quaternion.identity);
        play.GetComponent<PlayerAction>().turn = turnManager;
        turnManager.player = play;
        ui.player = play.GetComponent<PlayerController>();
        //Camera
        camera.GetComponent<PlayerFollow>().PlayerTransform = play.GetComponent<PlayerMovement>();

        //Companions
        GameObject companion = Instantiate(npc, new Vector3(1, heightSpawn, 0), Quaternion.identity);
        companion.GetComponent<MeshRenderer>().material = materialCompanions;
        companion.GetComponent<NpcAction>().turn = turnManager;
        turnManager.companions.Add(companion);
    }
}
