using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class Statistiques : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private TurnManager turn;
    private GenerateCharacters charGen;
    public int healthCurrent = 100;
    public int healthMax = 100;
    public int manaCurrent = 100;
    public int manaMax = 100;
    public int atq;
    public int def;
    public bool isDead = false;
    public int myTurn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    public void TakeDamage(int dmg)
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
            case "companion":

                break;
            case "enemy":
                //before death check if the gameobject is still referenced
                turn.enemies.Remove(gameObject);
                charGen.numberOfEnemy--;
                break;
        }
        Destroy(this.gameObject);
    }
}
