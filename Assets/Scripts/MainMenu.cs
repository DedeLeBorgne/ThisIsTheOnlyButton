using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1 );
    }

    public void Resume()
    {
        bool EnPause = GameObject.Find("FPSPlayer").gameObject.GetComponent<SC_FPSController>().isPaused;
        GameObject.Find("FPSPlayer").gameObject.GetComponent<SC_FPSController>().isPaused = !EnPause;
    }

    public void QuitGame()
    {
        Debug.Log("Quit !");
        Application.Quit();
    }
    

}
