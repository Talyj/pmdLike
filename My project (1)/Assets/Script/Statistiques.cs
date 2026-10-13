using UnityEngine;
using UnityEngine.Rendering;

public class Statistiques : MonoBehaviour
{
    [SerializeField] private TurnManagement turn;
    [SerializeField] private GenerateTerrain terrain;
    public int health = 100;
    public int mana = 100;
    public int atq;
    public int def;
    //private bool isDead = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turn = FindAnyObjectByType<TurnManagement>();
        terrain = FindAnyObjectByType<GenerateTerrain>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            //isDead = true;
            Dies();
        }
    }

    public void TakeDamage(int dmg)
    {
        var res = dmg - def;
        health -= res;
    }

    public void Dies()
    {
        //Multiple death scenario player, npc, enemy
        switch (gameObject.tag)
        {
            case "Player":

                break;
            case "companion":

                break;
            case "enemy":
                //before death check if the gameobject is still referenced
                turn.enemies.Remove(gameObject);
                terrain.numberOfEnemy--;
                break;
        }
        Destroy(this.gameObject);
    }
}
