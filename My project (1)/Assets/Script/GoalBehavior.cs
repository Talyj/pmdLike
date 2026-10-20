using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBehavior : MonoBehaviour
{
    public DataToSave data;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = FindAnyObjectByType<DataToSave>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerIsAround();
    }

    public void CheckIfPlayerIsAround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach(var col in hitColliders)
        {
            if(col.tag.ToLower() == "player")
            {
                //save character values
                //Change scene to go to village
                data.SaveData(col.gameObject);
                //Load the village scene
                SceneManager.LoadScene("Village");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (gameObject.tag.ToLower() == "goal")
        {
            Gizmos.color = Color.red;
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere(transform.position, 3f);
        }
    }
}
