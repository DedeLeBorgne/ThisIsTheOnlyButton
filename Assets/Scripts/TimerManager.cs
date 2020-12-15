using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private float timer = 0;
    private int minutes;
    private float seconds;
    // private bool hasAlreadyBeenTriggered = false;
    [SerializeField] private TextMeshProUGUI timerText;
    bool stopTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "";
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
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Bouton"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.GetComponent<ButtonScript>().buttonPressed = true;
                stopTimer = true;
            }
        }
    }
}
