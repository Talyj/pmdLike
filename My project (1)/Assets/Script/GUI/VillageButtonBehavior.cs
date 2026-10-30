using Assets.Script.Movements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VillageButtonBehavior : MonoBehaviour
{
    private Job job;
    private UI ui;
    private DataToSave data;
    private Statistiques stats;

    private void Start()
    {
        job = FindAnyObjectByType<Job>();
        ui = FindAnyObjectByType<UI>();
        data = FindAnyObjectByType<DataToSave>();
        stats = FindAnyObjectByType<Statistiques>();
    }
    public void ChangeJob()
    {
        //Add a check if the player has already a save with the job he changed into
        //if yes just take the value from the save
        // otherwise use the value from the default file
        var test = data.ReadDefaultSave();
        foreach(var res in test.jobs)
        {
            if(res.jobName == (Job.Jobs)int.Parse(gameObject.name))
            {
                job.job = res.jobName;
                stats.atq = res.atq;
                stats.def = res.def;
                stats.healthMax = res.healthMax;
                stats.healthCurrent = res.healthMax;
                stats.manaMax = res.manaMax;
                stats.manaCurrent = res.manaMax;
                stats.atqRange = res.atqRange;
                stats.detectRange = res.detectRange;
                StartCoroutine(ui.DisplaySkills());
                return;
            }
        }     
    }

    public void LeaveVillage()
    {
        data.SaveData(job.gameObject);
        SceneManager.LoadScene("BaseDungeon");
    }

    public void CloseUI(Canvas canvaToClose)
    {
        canvaToClose.gameObject.SetActive(false);
        canvaToClose.GetComponent<OpenUI>().isActive = false;
    }
}
