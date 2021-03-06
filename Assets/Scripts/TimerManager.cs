﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private float timer = 0;
    private int minutes;
    private float seconds;

    [SerializeField] private int currentLevel = 1;
    private int lastCorridorTriggered = 2;

// private bool hasAlreadyBeenTriggered = false;
[SerializeField] private TextMeshProUGUI timerText;
    bool stopTimer = true;

    private GameObject teleportGoal;
    private float respawnTimer;
    private float deadTime = 0.1f;
    private bool isDead = false;
    private int numberOfDeath = 0;

    private bool seesButtonMoving = false;
    private int index = 0;
    private bool buttonHasMoved = false;
    private float timerAddition = 0;

    private GameObject levelCounter ;

    private bool gameAlreadyWon = false;
    [SerializeField] private TextMeshProUGUI yourFinalTimeText;
    [SerializeField] private TextMeshProUGUI yourFinalDeathCountText;


    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "";
        // La fonction qui suit vient vérifier que les gameobjects qui doivent être désactivés pour que le code fonctionne (les "Activators") sont bien tous désactivés, et désactive ceux qui ne le sont pas.
        levelCounter = GameObject.Find("Levels");
        DeactivateAllActivators();
    }

    // Update is called once per frame
    void Update()
    {
        // Le timer avance constamment s'il n'est pas en mode "stop"
        if (stopTimer == false)
        {
            timer += Time.deltaTime;
            minutes = (int)timer / 60;
            seconds = timer % 60;
            // Debug.Log(minutes + ":" + seconds.ToString("00.00"));
            timerText.text = minutes + ":" + seconds.ToString("00.00");
        }
        
        // Si on est mort, alors on nous a téléporté et désactivé le character controller (check ligne ~129). Ici, on rétablit le character controller après un certain temps.
        if (isDead)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= deadTime)
            {
                this.GetComponent<SC_FPSController>().enabled = true;
                isDead = false;
                respawnTimer = 0;
            }
        }

        if (seesButtonMoving)
        {
            MoveButton();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Passer dans un trigger (placés à chaque début de niveau) relance le timer là où il s'était arrêté.
        if (other.gameObject.CompareTag("LanceurDeTimer"))
        {
            if (other.gameObject.GetComponent<TriggerTimer>().hasAlreadyBeenTriggered == false)
            {
                other.gameObject.GetComponent<TriggerTimer>().hasAlreadyBeenTriggered = true;
                stopTimer = false;

                // Dans certains niveaux, une règle spéciale s'applique également.
                switch (currentLevel)
                {
                    case 6:
                        this.gameObject.GetComponent<SC_FPSController>().invertedControls = true;
                        break;
                    case 10:
                        this.gameObject.GetComponent<SC_FPSController>().checkMovement = true;
                        break;
                    case 12:
                        seesButtonMoving = true;
                        break;
                    case 13:
                        this.gameObject.GetComponent<SC_FPSController>().checkRotation = true;
                        break;
                }
            }
        }

        // Quand on passe dans un couloir impair, on lance la fonction DeactivatePreviousLevelAndActivateNextLevel() avec lastCorridorTriggered = 2
        if (other.gameObject.CompareTag("ActivateurDeNiveauCouloir1"))
        {
            if (lastCorridorTriggered == 2)
            {
                DeactivatePreviousLevelAndActivateNextLevel(other);
                lastCorridorTriggered = 1;
            }

        }

        // Quand on passe dans un couloir pair, on lance la fonction DeactivatePreviousLevelAndActivateNextLevel() avec lastCorridorTriggered = 1
        if (other.gameObject.CompareTag("ActivateurDeNiveauCouloir2"))
        {
            if (lastCorridorTriggered == 1)
            {
                DeactivatePreviousLevelAndActivateNextLevel(other);
                lastCorridorTriggered = 2;
            }

        }

        if (other.gameObject.CompareTag("DeathZone"))
        {
            // Quand le joueur rentre dans une deathzone, on désactive le CharacterController (sinon ça crée une erreur au moment de la téléportation au checkpoint)
            this.GetComponent<SC_FPSController>().enabled = false;
            // ET on active isDead, qui va nous rendre le contrôle du characterController après un certain temps (cf Update)
            isDead = true;
            numberOfDeath++;

            // Si le joueur est mort est dans un niveau pair (dernier couloir traversé = corridor 1), alors on le fait respawner au Checkpoint situé à la fin du corridor 1 (et on reset sa position)
            if (lastCorridorTriggered == 1)
            {
                teleportGoal = GameObject.FindGameObjectWithTag("CheckpointRespawn1");
                this.gameObject.transform.position = teleportGoal.gameObject.transform.position;
                this.gameObject.transform.rotation = teleportGoal.gameObject.transform.rotation;
            }

            // Si le joueur est mort est dans un niveau impair (dernier couloir traversé = corridor 2), alors on le fait respawner au Checkpoint situé à la fin du corridor 2 (et on reset sa position)
            if (lastCorridorTriggered == 2)
            {
                teleportGoal = GameObject.FindGameObjectWithTag("CheckpointRespawn2");
                this.gameObject.transform.position = teleportGoal.gameObject.transform.position;
                this.gameObject.transform.rotation = teleportGoal.gameObject.transform.rotation;
            }
        }
    }

    private void DeactivatePreviousLevelAndActivateNextLevel(Collider other)
    {
        other.gameObject.transform.GetChild(0).gameObject.SetActive(true); // Active un mur qui empêche le joueur de faire demi-tour dans le couloir où il est actuellement
        // Désactivation du mur de l'autre couloir : 
        GameObject nextCorridor = GameObject.FindGameObjectWithTag("ActivateurDeNiveauCouloir" + lastCorridorTriggered);
        nextCorridor.gameObject.transform.GetChild(0).gameObject.SetActive(false); 

        // Désactivation du niveau précédent :
        GameObject previousLevel = GameObject.FindGameObjectWithTag("Niveau " + currentLevel);
        previousLevel.gameObject.SetActive(false);

        // Activation du niveau suivant :
        GameObject nextLevel = GameObject.FindGameObjectWithTag("Niveau " + (currentLevel + 1)).transform.GetChild(0).gameObject;
        nextLevel.gameObject.SetActive(true);

        // Keeping track of the current level the player is in
        currentLevel++;
    }

    private void DeactivateAllActivators()
    {

        for (int i = 1; i < levelCounter.transform.childCount; i ++)
        {
            // On va chercher l'activator de chaque niveau (à partir du niveau 2 car le niveau 1 n'en a pas et doit resté activer de base)
            GameObject levelToDisable = levelCounter.transform.GetChild(i).transform.GetChild(0).gameObject;
            // On le désactive
            levelToDisable.SetActive(false);
            // Debug.Log("Level disabled : " + levelToDisable);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Appuyer sur un bouton (placés à chaque fin de niveau) arrête le timer là où il s'était arrêté.
        if (other.gameObject.CompareTag("Bouton"))
        {
            // Si on se rapproche du bouton, le texte "Press E" s'active
            other.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.GetComponent<ButtonScript>().buttonPressed = true;
                stopTimer = true;
                // et les règles spéciales se désactivent.
                switch (currentLevel)
                {
                    case 6:
                        this.gameObject.GetComponent<SC_FPSController>().invertedControls = false;
                        break;
                    case 10:
                        this.gameObject.GetComponent<SC_FPSController>().checkMovement = false;
                        break;
                    case 12:
                        seesButtonMoving = false;
                        break;
                    case 13:
                        this.gameObject.GetComponent<SC_FPSController>().checkRotation = false;
                        break;

                }
                // Si le bouton sur lequel on a appuyé est le bouton du DERNIER niveau, alors on arrête le jeu. gameAlreayWon sert à vérifier qu'on arrête bien le jeu qu'une seule fois.
                if (currentLevel == levelCounter.transform.childCount)
                {
                    EndGame();
                    gameAlreadyWon = true;
                }
            }
        }

        // Appuyer sur un faux bouton...
        if (other.gameObject.CompareTag("FakeButton") || other.gameObject.CompareTag("TrapButton") || other.gameObject.CompareTag("TrapButton (floor)"))
        {
            // Si on se rapproche du faux bouton, le texte "Press E" s'active
            other.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.GetComponent<FakeButtonScript>().buttonPressed = true;
            }
        }
    }

    // Si on s'éloigne d'un bouton ou d'un faux bouton, le texte "Press E" se désactive
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bouton") || other.gameObject.CompareTag("FakeButton") || other.gameObject.CompareTag("TrapButton") || other.gameObject.CompareTag("TrapButton (floor)"))
        {
            other.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    // Dans le niveau 12, le bouton se déplace entre les 4 possibilités (rangées dans une array) toutes les 5 secondes
    private void MoveButton()
    {
        GameObject button = GameObject.FindGameObjectWithTag("Bouton").gameObject;
        GameObject[] positions = GameObject.FindGameObjectsWithTag("Possibilities");
        float timerInterval = 5.0f;

        if (!buttonHasMoved)
        {
            button.transform.position = positions[index].transform.position;
            buttonHasMoved = true;
           //  Debug.Log("Button moving");
            timerAddition = timer + timerInterval;
        }
          
        if (buttonHasMoved && timer > timerAddition && timerAddition != 0)
            {
                index++;
            // Debug.Log("Index increasing");
            buttonHasMoved = false;
                if (index == (positions.Length))
                {
                    index = 0;
                } 
            } 
    }

    private void EndGame()
    {
        if (!gameAlreadyWon)
        {
            // Debug.Log("Gagné !");
            // On désactive le character controller quand on gagne une partie. 
            this.GetComponent<SC_FPSController>().enabled = false;

            // On active le popup de fin.
            GameObject endGameMenu = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
            endGameMenu.SetActive(true);
            // On désactive le timer qui est en haut de l'écran...
            timerText.gameObject.SetActive(false);
            // ... et on affiche le contenu de ce timer dans le menu.
            yourFinalTimeText.text = minutes + ":" + seconds.ToString("00.00");

            // Le texte indiquant le nombre de morts est légèrement différent en fonction du nombre de morts.
            if (numberOfDeath == 0)
            {
                yourFinalDeathCountText.text = "(AND YOU DIDN'T DIE EVEN ONCE !)";

            } else if (numberOfDeath == 1)
            {
                yourFinalDeathCountText.text = "(and you died " + numberOfDeath + " time.)";
            } else if (numberOfDeath > 1)
            {
                yourFinalDeathCountText.text = "(and you died " + numberOfDeath + " times.)";
            }

            // On rend le contrôle et la visibilité de la souris.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // On envoie les informations du score de la partie en cours au script "Navette" qui est persistant entre les scènes, pour que le score puisse apparaître dans le leaderboard du menu principal.
            GameObject.Find("HighScoreManagerStorer").GetComponent<LeaderboardStoredScore>().time = timer;
            GameObject.Find("HighScoreManagerStorer").GetComponent<LeaderboardStoredScore>().deathCount = numberOfDeath;

        }


    }
}
