using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LeaderboardStoredScore : MonoBehaviour
{
    public float time = 0;
    public int deathCount;
    private bool isFullWithAScore = false;
    [SerializeField] private float currentBestTime = 1800;
    private int currentBestTimeDeathCount;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (GameObject.FindGameObjectsWithTag("HighScoreManagerStorer").Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
       

        if (time != 0 )
        {
            isFullWithAScore = true;

            // Create a temporary reference to the current scene.
            Scene currentScene = SceneManager.GetActiveScene();

            // Retrieve the name of this scene.
            string sceneName = currentScene.name;

            if (sceneName == "Menu" && isFullWithAScore)
            {
                isFullWithAScore = false;

                if (time < currentBestTime)
                {
                    Debug.Log("Arrivé sur le menu. Délivrage du nouveau meilleur temps...");
                    GameObject.Find("Canvas").transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetComponent<LeaderboardManager>().updateLeaderboard(time, deathCount);
                    currentBestTime = time;
                    currentBestTimeDeathCount = deathCount;
                    time = 0;
                    deathCount = 0;
                } else
                {
                    Debug.Log("Arrivé sur le menu. Délivrage de l'ancien meilleur temps...");
                    GameObject.Find("Canvas").transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetComponent<LeaderboardManager>().updateLeaderboard(currentBestTime, currentBestTimeDeathCount);
                    time = 0;
                    deathCount = 0;
                }
                

            }
        }
    }
}
