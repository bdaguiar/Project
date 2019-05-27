using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inGameUI : MonoBehaviour
{
    private GameObject panel;
    private void Start()
    {
        panel = GameObject.Find("Panel");
        panel.SetActive(false);
    }
    public void openMenu()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);
    }

    public void closeMenu()
    {
        Time.timeScale = 1f;
        panel.SetActive(false);
    }

    public void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }

    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
