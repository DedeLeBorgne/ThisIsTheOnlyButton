using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private float timer = 0;
    private int minutes;
    private float seconds;

    private int currentLevel = 1;
    private int lastCorridorTriggered = 2;

// private bool hasAlreadyBeenTriggered = false;
[SerializeField] private TextMeshProUGUI timerText;
    bool stopTimer = true;

    private GameObject teleportGoal;
    private float respawnTimer;
    private float deadTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "";
        // La fonction qui suit vient vérifier que les gameobjects qui doivent être désactivés pour que le code fonctionne (les "Activators") sont bien tous désactivés, et désactive ceux qui ne le sont pas.
        // DeactivateAllActivators();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer == false)
        {
            timer += Time.deltaTime;
            minutes = (int)timer / 60;
            seconds = timer % 60;
            // Debug.Log(minutes + ":" + seconds.ToString("00.00"));
            timerText.text = minutes + ":" + seconds.ToString("00.00");
        }
        /*if (timer > 5 && !stopTimer)
         {
             stopTimer = true;
         }*/

        if (timer >= respawnTimer)
        {
            this.GetComponent<SC_FPSController>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LanceurDeTimer"))
        {
            if (other.gameObject.GetComponent<TriggerTimer>().hasAlreadyBeenTriggered == false)
            {
                other.gameObject.GetComponent<TriggerTimer>().hasAlreadyBeenTriggered = true;
                stopTimer = false;
            }
        }

        if (other.gameObject.CompareTag("ActivateurDeNiveauCouloir1"))
        {
            if (lastCorridorTriggered == 2)
            {
                DeactivatePreviousLevelAndActivateNextLevel(other);
                lastCorridorTriggered = 1;
            }

        }

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
            // ET on rend respawntimer supérieur à timer. Or, le charactercontroller ne s'active que si le timer est supérieur au respawntimer (cf Update)
            // Donc on définit aussi combien de temps il sera mort (ne pourra plus se déplacer)
            respawnTimer = timer + deadTime;

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

        currentLevel++;
    }

    private void DeactivateAllActivators()
    {
        GameObject levelCounter = GameObject.Find("Levels");

        for (int i = 1; i < levelCounter.transform.childCount; i ++)
        {
            // On va chercher l'activator de chaque niveau (à partir du niveau 2)
            GameObject levelToDisable = levelCounter.transform.GetChild(i).transform.GetChild(0).gameObject;
            // On le désactive
            levelToDisable.SetActive(false);
            // Debug.Log("Level disabled : " + levelToDisable);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Bouton"))
        {
            other.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.GetComponent<ButtonScript>().buttonPressed = true;
                stopTimer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bouton"))
        {
            other.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
