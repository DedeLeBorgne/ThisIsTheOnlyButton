using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ButtonScript : MonoBehaviour
{
    public bool buttonPressed = false;
    private float pushButtonTimer = 0;
    private float pushButtonTimerMax = 0.2f;
    private float pushButtonTimerMaxUp = 0.35f;
    private float buttonMoveSpeed = 0.3f;
    private float wallMoveSpeed = 1;
    private float yCoordinateWhereWallIsBelowGround = -2.5f;
    public TextMeshPro pressEText;
    private GameObject wallLinkedToButton;


    // Start is called before the first frame update
    void Start()
    {
        wallLinkedToButton = GameObject.FindGameObjectWithTag("WallLinkedToButton");
    }


    // Update is called once per frame
    void Update()
    {
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
