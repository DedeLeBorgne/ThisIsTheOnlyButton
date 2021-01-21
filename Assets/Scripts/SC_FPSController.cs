using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float Speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f; //permet de ne pas regarder trop haut ni trop bas (cou)

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool invertedControls = false;
    public bool checkMovement = false;
    public bool checkRotation = false;

    public bool isPaused = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Bloquer le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        

        // Mettre le jeu en pause
        if(Input.GetKeyDown(KeyCode.Escape))
            isPaused = !isPaused;

        if(isPaused)
        {
            //Debug.Log("Jeu en Pause");
            Time.timeScale = 0f;
            JeuEnPause();
        }
        else
        {
            Time.timeScale = 1f;
            GameObject MenuPause = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
            MenuPause.SetActive(false);
            Start();

            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float currentSpeedForward = Speed * Input.GetAxis("Vertical");
            float currentSpeedHorizontal = Speed * Input.GetAxis("Horizontal");

            float movementDirectionY = moveDirection.y;

            if (!invertedControls)
            {
                moveDirection = (forward * currentSpeedForward) + (right * currentSpeedHorizontal);
            } else if (invertedControls)
            {
                moveDirection = (-forward * currentSpeedForward) + (-right * currentSpeedHorizontal);
            }

            if (checkMovement)
            {
                GameObject button = GameObject.FindGameObjectWithTag("Niveau10Activator").gameObject.transform.GetChild(0).gameObject;
                if (moveDirection == Vector3.zero)
                {
                    button.gameObject.SetActive(true);
                }
                else {
                    button.gameObject.SetActive(false);
                }
            }

            if (checkRotation)
            {
                GameObject button = GameObject.FindGameObjectWithTag("Niveau13Activator").gameObject.transform.GetChild(0).gameObject;
                float rotationAngleAuthorized = 0.37f;

                if (this.transform.rotation.y < -rotationAngleAuthorized || this.transform.rotation.y > rotationAngleAuthorized || this.transform.position.z < -11.19f)
                {
                    // Debug.Log("WRONG ! rotation : " + transform.rotation.y);
                    button.gameObject.SetActive(false);
                } else
                {
                     // Debug.Log("GOOD ! rotation : " + transform.rotation.y);
                    button.gameObject.SetActive(true);
                }
            }



            if (Input.GetButton("Jump") && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2).
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            }
    }

    void JeuEnPause(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject MenuPause = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        MenuPause.SetActive(true);
    }
}