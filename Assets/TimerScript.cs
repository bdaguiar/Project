using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public double timer = 0.0;
    public int kills = 0;

    
    private void Update()
    {
        
        timer += Time.deltaTime;
        
        GameObject.Find("Time").GetComponent<Text>().text = timer.ToString("F2") ;
        GameObject.Find("Kills").GetComponent<Text>().text = "Kills: " + kills;
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("timer", (float)timer);
    }

    
}
