using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool buttonPressed = false;
    private float pushButtonTimer = 0;
    private float pushButtonTimerMax = 0.2f;
    private float pushButtonTimerMaxUp = 0.35f;
    private float buttonMoveSpeed = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            pushButtonTimer += Time.deltaTime;
            if (pushButtonTimer <= pushButtonTimerMax)
            {
                gameObject.transform.GetChild(1).Translate(Vector3.down * Time.deltaTime * buttonMoveSpeed);
            }
            if (pushButtonTimer > pushButtonTimerMax)
            {
                gameObject.transform.GetChild(1).Translate(Vector3.up * Time.deltaTime * buttonMoveSpeed);
            }
            if (pushButtonTimer > pushButtonTimerMaxUp)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
