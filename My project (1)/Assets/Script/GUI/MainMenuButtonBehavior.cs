using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    int n;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnButtonPressPlay()
    {
        Debug.Log("Play");
        SceneManager.LoadScene(1);
    }

    public void OnButtonPressQuit()
    {
        Debug.Log("quit");
        //quit
        Application.Quit();
    }
}
