using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LeaderboardStoredScore : MonoBehaviour
{
    // Ce code fait office de "Navette" entre TimerManager (scène "game") qui vient récupérer le temps final du joueur en fin de partie,
    // et LeaderboardManager (scène "Menu") qui affiche ce temps dans le classement.
    // Ce script est sur un objet qui est perpétuel entre les deux scènes.
    public float time = 0;
    public int deathCount;
    private bool isFullWithAScore = false;
    private float currentBestTime = 1800;
    private int currentBestTimeDeathCount;

    // Start is called before the first frame update
    void Awake()
    {
        // Permet de rendre l'objet perpétuel entre les deux scènes.
        DontDestroyOnLoad(this);

        // Ce if permet d'éviter un bug qui dublique l'objet à chaque fois qu'on recharge la scène "Menu"
        if (GameObject.FindGameObjectsWithTag("HighScoreManagerStorer").Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        // TimerManager va update les variables publiques (time et deathcount), on vérifie dans le update dès qu'elles ont bougé 
        if (time != 0 )
        {
            isFullWithAScore = true;

            // Ici on vérifie si on vient de battre son personal best score ou non. Si c'est le cas, on l'enregistre, sinon osef car on ne veut retenir que le personal best
            // DONC, si on a battu son personal best score, on affichera ce nouveau score dans le classement ET SINON on affichera l'ancien personal best score dans le classement
            if (time < currentBestTime)
            {
                currentBestTime = time;
                currentBestTimeDeathCount = deathCount;
            }

           // Create a temporary reference to the current scene.
           Scene currentScene = SceneManager.GetActiveScene();

            // Retrieve the name of this scene.
            string sceneName = currentScene.name;

            // On attend de revenir sur la scène "Menu pour délivrer le score dans le tableau des scores
            if (sceneName == "Menu" && isFullWithAScore)
            {
                isFullWithAScore = false;

                // Affichage du personal best score dans le tableau
                GameObject.Find("Canvas").transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetComponent<LeaderboardManager>().updateLeaderboard(currentBestTime, currentBestTimeDeathCount);
                
                // Reset les champs (pas particulièrement utile mais bon on sait jamais)
                time = 0;
                deathCount = 0;
            }
        }
    }

}
