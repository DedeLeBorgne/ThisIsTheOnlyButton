using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ButtonScript : MonoBehaviour
{
    // Ce script est assigné aux boutons qui OUVRENT LA PORTE DE SORTIE DE CHAQUE NIVEAU.
    public bool buttonPressed = false;
    private float pushButtonTimer = 0;
    private float pushButtonTimerMax = 0.2f;
    private float pushButtonTimerMaxUp = 0.35f;
    private float buttonMoveSpeed = 0.3f;
    private float wallMoveSpeed = 1;
    private float yCoordinateWhereWallIsBelowGround = -2.5f;
    public TextMeshPro pressEText;
    private GameObject wallLinkedToButton;
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        wallLinkedToButton = GameObject.FindGameObjectWithTag("WallLinkedToButton");
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        // Si on appuie sur le bouton, le buzzer rouge descend et remonte (petite animation) et la porte qui y est associée se déplace également.
        if (buttonPressed)
        {
            pushButtonTimer += Time.deltaTime;
            pressEText.gameObject.SetActive(false);
            if (pushButtonTimer <= pushButtonTimerMax)
            {
                gameObject.transform.GetChild(1).Translate(Vector3.down * Time.deltaTime * buttonMoveSpeed);
            }
            if (pushButtonTimer > pushButtonTimerMax)
            {
                wallLinkedToButton.transform.Translate(Vector3.down * Time.deltaTime * wallMoveSpeed);
                gameObject.transform.GetChild(1).Translate(Vector3.up * Time.deltaTime * buttonMoveSpeed);
            }
            if (pushButtonTimer > pushButtonTimerMaxUp)
            {
                buttonMoveSpeed = 0;
            }
            if (wallLinkedToButton.transform.position.y < yCoordinateWhereWallIsBelowGround)
            {
                wallMoveSpeed = 0;
                Destroy(gameObject);
            }
        }

        // Animation du texte "Press E" qui tourne
        if (pressEText.isActiveAndEnabled)
        {
            pressEText.transform.Rotate(0, 1, 0);
            /*        pressEText.transform.Rotate(0, Vector3.right * 1, 0);
                    Vector3 namePose = Camera.main.WorldToScreenPoint(this.transform.position);
                    pressEText.transform.position = namePose;

                    gameObject.transform.GetChild(2).transform.Translate = ;*/
        }

        

    }
}
