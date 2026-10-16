using UnityEngine;

public class SimpleAttack : MonoBehaviour
{
    private PlayerAction action;

    private void Start()
    {
        action = GetComponent<PlayerAction>();
    }
    public void DoYourThing(float range, int dmg)
    {
        //Race skill should be put there like
        //if(race == gluant){
        //Change something
        //}
        //else if(race == goblin){
        // change something else
        //}

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.collider.tag.ToLower() == "enemy")
            {
                hit.collider.GetComponent<Statistiques>().TakeDamage(dmg);
            }
        }
        action.ChangeNpcState();
        action.isActing = false;
        action.turn.TurnChange();
    }
}
