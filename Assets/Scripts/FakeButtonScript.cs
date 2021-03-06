﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FakeButtonScript : MonoBehaviour
{
    // Ce script est assigné aux boutons qui n'ouvrent PAS la porte de sortie. Ils peuvent soit ne rien faire (dans ce cas ils sont taggués "FakeButton") soit ouvrir autre chose (tag "TrapButton")
    public bool buttonPressed = false;
    private float pushButtonTimer = 0;
    private float pushButtonTimerMax = 0.2f;
    private float pushButtonTimerMaxUp = 0.35f;
    private float buttonMoveSpeed = 0.3f;
    private float trapMoveSpeed = 1;
    private float yCoordinateWhereWallIsBelowGround = -2.5f;
    public TextMeshPro pressEText;
    private GameObject trapLinkedToButton;


    // Start is called before the first frame update
    void Start()
    {
        if (this.CompareTag("TrapButton") || this.CompareTag("TrapButton (floor)"))
        {
            trapLinkedToButton = GameObject.FindGameObjectWithTag("trapLinkedToButton");
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Si on appuie sur le bouton, le buzzer rouge descend et remonte (petite animation) et le piège qui y est éventuellement associé se déplace également.
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
                if (this.CompareTag("TrapButton"))
                {
                    trapLinkedToButton.transform.Translate(Vector3.down * Time.deltaTime * trapMoveSpeed);
                }
                gameObject.transform.GetChild(1).Translate(Vector3.up * Time.deltaTime * buttonMoveSpeed);
            }
            if (pushButtonTimer > pushButtonTimerMaxUp)
            {
                buttonMoveSpeed = 0;
                if (this.CompareTag("FakeButton"))
                {
                    trapMoveSpeed = 0;
                    Destroy(gameObject);
                } else if (this.CompareTag("TrapButton") && trapLinkedToButton.transform.position.y < yCoordinateWhereWallIsBelowGround)
                {
                    trapMoveSpeed = 0;
                    Destroy(gameObject);
                } else if (this.CompareTag("TrapButton (floor)"))
                {
                    trapLinkedToButton.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
                
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
