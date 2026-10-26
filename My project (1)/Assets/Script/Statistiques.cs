using Assets.Script.Movements;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class Statistiques : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private TurnManager turn;
    private DamageOverTime dot;
    private GenerateCharacters charGen;

    public float healthCurrent;
    public float healthMax;
    public float manaCurrent;
    public float manaMax;
    public float atq;
    public float def;
    public bool isDead = false;
    public int myTurn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthCurrent = healthMax;
        manaCurrent = manaMax;
        turn = FindAnyObjectByType<TurnManager>();
        dot = FindAnyObjectByType<DamageOverTime>();
        charGen = FindAnyObjectByType<GenerateCharacters>();
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(healthCurrent, healthMax);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        if(healthBar != null)
        {
            healthBar.LookAtCamera();
        }
    }

    private void CheckHealth()
    {
        if(healthCurrent <= 0)
        {
            //isDead = true;
            Dies();
        }
    }

    //Maybe add a takePoisonDamage
    public void TakeDamage(float dmg)
    {
        var res = dmg - def;
        healthCurrent -= res;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(healthCurrent, healthMax);
        }
    }

    public void Dies()
    {
        //Multiple death scenario player, npc, enemy
        switch (gameObject.tag)
        {
            case "Player":
                isDead = true;
                return;
            case "characters":
                dot.listCharacterToDebuff.Remove(gameObject);
                turn.companions.Remove(gameObject);
                break;
            case "enemy":
                //before death check if the gameobject is still referenced
                turn.TurnChange();
                //add the list of dot
                dot.listCharacterToDebuff.Remove(gameObject);
                turn.enemies.Remove(gameObject);
                charGen.numberOfEnemy--;
                break;
        }
        Destroy(this.gameObject);
    }

    public void GetStats()
    {
        var fileName = Application.persistentDataPath + "/save.json";
        Player savedPlayer;
        if (File.Exists(fileName))
        {
            string jsonString = File.ReadAllText(Application.persistentDataPath + "/save.json");
            savedPlayer = JsonUtility.FromJson<Player>(jsonString);

            atq = savedPlayer.atq;
            def = savedPlayer.def;
            healthMax = savedPlayer.healthMax;
            manaMax = savedPlayer.manaMax;
            GetComponent<Job>().job = savedPlayer.job;

            atq = savedPlayer.atq;
            atq = savedPlayer.atq;
        }
    }
}
