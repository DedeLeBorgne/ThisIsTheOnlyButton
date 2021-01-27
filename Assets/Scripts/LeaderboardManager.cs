using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject scorePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void updateLeaderboard(float timeInSeconds, int numberOfDeath)
    {
        GameObject leaderBoard = this.gameObject;

        GameObject newScorePrefab = leaderBoard.transform.GetChild(0).gameObject;

        /*// Création d'une nouvelle ligne de score dans le tableau des scores
        GameObject newScorePrefab = Instantiate(scorePrefab);

        // Reset de la position, de la rotation et de la scale de la nouvelle ligne de score, et assignation en tant qu'enfant du leaderboard
        newScorePrefab.transform.SetParent(leaderBoard.transform);
        newScorePrefab.transform.rotation = new Quaternion(0, 0, 0, 0);
        newScorePrefab.transform.localScale = new Vector3(1, 1, 1);
        newScorePrefab.transform.position = leaderBoard.transform.position + new Vector3(0, -5f, 0);*/

        // Placement de la ligne de score exactement au bon endroit
        float yMarginBetweenEachLine = -2.17f;
        bool oneNewPersonInLeaderboard = false;
        // Pour chaque position dans le placement...
        for (int i = 0; i < leaderBoard.transform.childCount-1; i++)
        {
            // On vérifie si le temps du joueur bat cette position et SI OUI, on met son temps à cette position 
            if (timeInSeconds <= leaderBoard.transform.GetChild(i).transform.GetComponent<LeaderboardValue>().time)
            {
                oneNewPersonInLeaderboard = true;
                newScorePrefab.SetActive(true);
                Debug.Log("The personal best score is at position " + (i));
                // newScorePrefab.transform.position += new Vector3(0, i * yMarginBetweenEachLine, 0);
                newScorePrefab.transform.Translate(Vector3.down * (i * -yMarginBetweenEachLine), Space.World);
                Debug.Log("Its position is at " + newScorePrefab.transform.position);

                newScorePrefab.transform.GetComponent<LeaderboardValue>().time = timeInSeconds;
                newScorePrefab.transform.GetComponent<LeaderboardValue>().pseudo = "YOUR PERSONAL BEST";
                newScorePrefab.transform.GetComponent<LeaderboardValue>().deathCount = numberOfDeath;

                // Si le temps du joueur s'est incrusté dans le classement, alors on décale toutes les positions inférieures à la sienne de 5 unités sur l'axe y.
                // Cela permet d'éviter que le temps du joueur overlap un autre (j'aurais mieux fait d'utiliser des tableaux je crois, oups)
                for (int y = i; y < leaderBoard.transform.childCount; y++)
                {
                    if (leaderBoard.transform.GetChild(y).transform.GetComponent<LeaderboardValue>().time > timeInSeconds)
                    {
                        // Debug.Log("Décalage de " + leaderBoard.transform.GetChild(y).transform.GetComponent<LeaderboardValue>().time + " - " + leaderBoard.transform.GetChild(y).transform.GetComponent<LeaderboardValue>().pseudo);
                        leaderBoard.transform.GetChild(y).transform.position += new Vector3(0, yMarginBetweenEachLine, 0);
                    }
                  
                }
                // Si jamais on a battu un temps, alors on arrête de chercher si on a battu d'autres temps, car les autres sont forcément inférieurs.
                break;
            }
        }

        // Suppression du pire score du leaderboard si un nouveau score est entré
        if (oneNewPersonInLeaderboard)
        {
           

            GameObject currentWorstTime = leaderBoard.transform.GetChild(0).gameObject;

            for (int i = 0; i < leaderBoard.transform.childCount; i++) 
            {
                if (leaderBoard.transform.GetChild(i).transform.GetComponent<LeaderboardValue>().time > currentWorstTime.transform.GetComponent<LeaderboardValue>().time)
                {
                    currentWorstTime = leaderBoard.transform.GetChild(i).gameObject;
                }
            }

            // Debug.Log("Destroying " + currentWorstTime.transform.GetComponent<LeaderboardValue>().time + " - " + currentWorstTime.transform.GetComponent<LeaderboardValue>().pseudo);
            Destroy(currentWorstTime);
        }



        // Debug.Log("Done");
    }
}
