using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuButtons : MonoBehaviour
{

    private void Start()
    {
        GameObject.Find("character").GetComponent<Animator>().SetBool("isGrounded", true);
        GameObject.Find("character").GetComponent<Animator>().SetBool("isMoving", true);
    }
    

    public void play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void score()
    {
        PlayerPrefs.SetInt("fromGame", 0);
        SceneManager.LoadScene("ScoreBoard");
    }

    public void about()
    {
        SceneManager.LoadScene("About");
    }

    public void settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
