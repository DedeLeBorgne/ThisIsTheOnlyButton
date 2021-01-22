using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardValue : MonoBehaviour
{
    public float time;
    public string pseudo;
    public int deathCount;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI deathCountText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        timeText.text = minutes + ":" + seconds.ToString("00.00");
        nameText.text = pseudo;
        deathCountText.text = deathCount.ToString();
    }
}
