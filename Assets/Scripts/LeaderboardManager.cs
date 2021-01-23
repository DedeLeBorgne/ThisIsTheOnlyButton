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

        // Création d'une nouvelle ligne de score
        GameObject newScorePrefab = Instantiate(scorePrefab);

        // Reset de la position, de la rotation et de la scale de la nouvelle ligne de score, et assignation en tant qu'enfant du leaderboard
        newScorePrefab.transform.SetParent(leaderBoard.transform);
        newScorePrefab.transform.rotation = new Quaternion(0, 0, 0, 0);
        newScorePrefab.transform.localScale = new Vector3(1, 1, 1);
        newScorePrefab.transform.position = leaderBoard.transform.position + new Vector3(0, -5f, 0);

        // Placement de la ligne de score exactement au bon endroit
        float yMarginBetweenEachLine = -5f;
        bool oneNewPersonInLeaderboard = false;
        for (int i = 0; i < leaderBoard.transform.childCount-1; i++)
        {
            if (timeInSeconds <= leaderBoard.transform.GetChild(i).transform.GetComponent<LeaderboardValue>().time)
            {
                oneNewPersonInLeaderboard = true;
                Debug.Log("Placing a new score at position " + (i+1));
                newScorePrefab.transform.position += new Vector3(0, i * yMarginBetweenEachLine, 0);

                newScorePrefab.transform.GetComponent<LeaderboardValue>().time = timeInSeconds;
                newScorePrefab.transform.GetComponent<LeaderboardValue>().pseudo = "YOUR PERSONAL BEST";
                newScorePrefab.transform.GetComponent<LeaderboardValue>().deathCount = numberOfDeath;

                for (int y = i; y < leaderBoard.transform.childCount; y++)
                {
                    if (leaderBoard.transform.GetChild(y).transform.GetComponent<LeaderboardValue>().time > timeInSeconds)
                    {
                        // Debug.Log("Décalage de " + leaderBoard.transform.GetChild(y).transform.GetComponent<LeaderboardValue>().time + " - " + leaderBoard.transform.GetChild(y).transform.GetComponent<LeaderboardValue>().pseudo);
                        leaderBoard.transform.GetChild(y).transform.position += new Vector3(0, yMarginBetweenEachLine, 0);
                    }
                  
                }
                break;
            }
        }

        //Décalage de tous les scores inférieurs au nouveau score + Suppression du pire score du leaderboard si un nouveau score est entré
        if (oneNewPersonInLeaderboard)
        {
           

            GameObject currentWorstTime = leaderBoard.transform.GetChild(0).gameObject;

            for (int i = 0; i < leaderBoard.transform.childCount - 1; i++) 
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
