using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class Statistiques : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private TurnManager turn;
    private GenerateCharacters charGen;
    public float healthCurrent = 100;
    public float healthMax = 100;
    public float manaCurrent = 100;
    public float manaMax = 100;
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
                turn.companions.Remove(gameObject);
                break;
            case "enemy":
                //before death check if the gameobject is still referenced
                //add the list of dot
                turn.enemies.Remove(gameObject);
                charGen.numberOfEnemy--;
                break;
        }
        Destroy(this.gameObject);
    }
}
