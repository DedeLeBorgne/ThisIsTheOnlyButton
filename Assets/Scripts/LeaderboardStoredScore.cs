using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LeaderboardStoredScore : MonoBehaviour
{
    public float time = 0;
    public int deathCount;
    private bool isFullWithAScore = false;
    private float bestTime;
    private int bestTimeDeathCount;
         

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
       

        if (time != 0 && time > bestTime)
        {
            isFullWithAScore = true;

            // Create a temporary reference to the current scene.
            Scene currentScene = SceneManager.GetActiveScene();

            // Retrieve the name of this scene.
            string sceneName = currentScene.name;

            if (sceneName == "Menu" && isFullWithAScore)
            {
                isFullWithAScore = false;
                GameObject.Find("Canvas").transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetComponent<LeaderboardManager>().updateLeaderboard(time, deathCount);
                bestTime = time;
                bestTimeDeathCount = deathCount;
                time = 0;
                deathCount = 0;

            }
        }
    }
}
