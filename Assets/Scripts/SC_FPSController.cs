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

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Bloquer le curseur
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
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