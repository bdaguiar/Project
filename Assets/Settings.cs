using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private GameObject prompt;
    private void Start()
    {
        prompt = GameObject.Find("Prompt");
        prompt.SetActive(false);
    }
    public void resetButton()
    {
        prompt.SetActive(true);
    }
    public void resetScores()
    {
        PlayerPrefs.SetFloat("maxTimer", 0);
        PlayerPrefs.SetFloat("maxHigh", 0);
        PlayerPrefs.SetFloat("maxDeep", 0);
        PlayerPrefs.SetFloat("maxDelta", 0);
        PlayerPrefs.SetInt("maxJump", 0);
        PlayerPrefs.SetInt("maxDive", 0);
        PlayerPrefs.SetInt("maxSmash", 0);
        PlayerPrefs.SetInt("maxSlide", 0);
        PlayerPrefs.SetInt("maxKills", 0);
        GameObject.Find("Text").GetComponent<Text>().text = "High Scores were deleted";
        GameObject.Find("cancelText").GetComponent<Text>().text = "Ok";
        GameObject.Find("Yes").SetActive(false);
    }
    public void cancel()
    {
        prompt.SetActive(false);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
