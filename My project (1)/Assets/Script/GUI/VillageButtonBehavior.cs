using Assets.Script.Movements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VillageButtonBehavior : MonoBehaviour
{
    private Job job;
    private UI ui;
    private DataToSave data;

    private void Start()
    {
        job = FindAnyObjectByType<Job>();
        ui = FindAnyObjectByType<UI>();
        data = GetComponent<DataToSave>();
    }
    public void ChangeJob()
    {        
        job.job = (Job.Jobs)int.Parse(gameObject.name);
        StartCoroutine(ui.DisplaySkills());
    }

    public void LeaveVillage()
    {
        data.SaveData(job.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
