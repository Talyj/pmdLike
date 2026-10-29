using UnityEditor.Animations;
using UnityEngine;

public class HealAnimation : MonoBehaviour
{

    [SerializeField] private GameObject healAnimation;
    [SerializeField] private AnimatorController healController;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnVfx();
        }
    }

    public void SpawnVfx()
    {
        GameObject goal = Instantiate(healAnimation, new Vector3(0,0,0), Quaternion.identity);
        goal.GetComponent<Animator>().runtimeAnimatorController = healController;
    }
}
