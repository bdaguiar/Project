using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Scores : MonoBehaviour
{
    public float timer, high, deep, delta, maxTimer, maxHigh, maxDeep, maxDelta;
    public int jump, maxJump, dive, maxDive, smash, maxSmash, slide, maxSlide, kills, maxKills, fromGame;
    private bool newHighScore = false;

    private bool isProcessing = false, isFocus = false;

    private void Start()
    {
        timer = PlayerPrefs.GetFloat("timer");
        high = PlayerPrefs.GetFloat("high");
        deep = PlayerPrefs.GetFloat("deep");
        delta = PlayerPrefs.GetFloat("delta");
        jump = PlayerPrefs.GetInt("jump");
        dive = PlayerPrefs.GetInt("dive");
        smash = PlayerPrefs.GetInt("smash");
        slide = PlayerPrefs.GetInt("slide");
        kills = PlayerPrefs.GetInt("kills");
        fromGame = PlayerPrefs.GetInt("fromGame");

        maxTimer = PlayerPrefs.GetFloat("maxTimer", 0);
        maxHigh = PlayerPrefs.GetFloat("maxHigh", 0);
        maxDeep = PlayerPrefs.GetFloat("maxDeep", 0);
        maxDelta = PlayerPrefs.GetFloat("maxDelta", 0);
        maxJump = PlayerPrefs.GetInt("maxJump", 0);
        maxDive = PlayerPrefs.GetInt("maxDive", 0);
        maxSmash = PlayerPrefs.GetInt("maxSmash", 0);
        maxSlide = PlayerPrefs.GetInt("maxSlide", 0);
        maxKills = PlayerPrefs.GetInt("maxKills", 0);

        fillTexts();
        highScores();
        if (newHighScore) saveScores();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

//--------------------------------------------------------

    public void share()
    {
        if(!isProcessing)
        {
            StartCoroutine(ShareScreenshot());
        }
    }
    
    IEnumerator ShareScreenshot()
    {
        isProcessing = true;
        yield return new WaitForEndOfFrame();

        string myFolderLocation = "/storage/emulated/0/Images/Project/";

        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }

        ScreenCapture.CaptureScreenshot("screenshot.png", 1);
        string destination = "/storage/emulated/0/Android/data/com.bdaguiar.Project/files/screenshot.png";

        yield return new WaitForSecondsRealtime(0.3f);
        if (!Application.isEditor)
        {
            
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", destination);
            
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Can you beat my high scores?\nDownload the app in https://drive.google.com/uc?id=1osykENHx312HvWhv70gXy7Kqr2bcfb5g&export=download");
            intentObject.Call<AndroidJavaObject>("setType", "image/*");
            
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high scores");

            currentActivity.Call("startActivity", intentObject);
            yield return new WaitForSecondsRealtime(1);
        }
        yield return new WaitUntil(() => isFocus);
        
        isProcessing = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

    //----------------------------------------------------



    public void fillTexts()
    {
        if(fromGame==1)
        {
            GameObject.Find("tJump").GetComponent<Text>().text = jump.ToString();
            GameObject.Find("tDive").GetComponent<Text>().text = dive.ToString();
            GameObject.Find("tSmash").GetComponent<Text>().text = smash.ToString();
            GameObject.Find("tSlide").GetComponent<Text>().text = slide.ToString();
            GameObject.Find("tHigh").GetComponent<Text>().text = high.ToString();
            GameObject.Find("tDeep").GetComponent<Text>().text = deep.ToString();
            GameObject.Find("tDelta").GetComponent<Text>().text = delta.ToString();
            GameObject.Find("tTime").GetComponent<Text>().text = timer.ToString();
            GameObject.Find("tKilled").GetComponent<Text>().text = kills.ToString();
        }
        else
        {
            GameObject.Find("tJump").GetComponent<Text>().text = maxJump.ToString();
            GameObject.Find("tDive").GetComponent<Text>().text = maxDive.ToString();
            GameObject.Find("tSmash").GetComponent<Text>().text = maxSmash.ToString();
            GameObject.Find("tSlide").GetComponent<Text>().text = maxSlide.ToString();
            GameObject.Find("tHigh").GetComponent<Text>().text = maxHigh.ToString();
            GameObject.Find("tDeep").GetComponent<Text>().text = maxDeep.ToString();
            GameObject.Find("tDelta").GetComponent<Text>().text = maxDelta.ToString();
            GameObject.Find("tTime").GetComponent<Text>().text = maxTimer.ToString();
            GameObject.Find("tKilled").GetComponent<Text>().text = maxKills.ToString();
        }
    }

    public void highScores()
    {
        if (jump > maxJump)
        {
            maxJump = jump;
            newHighScore = true;
        }
        if (dive > maxDive)
        {
            maxDive = dive;
            newHighScore = true;
        }
        if (smash > maxSmash)
        {
            maxSmash = smash;
            newHighScore = true;
        }
        if (slide > maxSlide)
        {
            maxSlide = slide;
            newHighScore = true;
        }
        if (high > maxHigh)
        {
            maxHigh = high;
            newHighScore = true;
        }
        if (deep < maxDeep)
        {
            maxDeep = deep;
            newHighScore = true;
        }
        if (delta > maxDelta)
        {
            maxDelta = delta;
            newHighScore = true;
        }
        if (kills > maxKills)
        {
            
            maxKills = kills;
            newHighScore = true;
        }
        if (timer > maxTimer)
        {
            maxTimer = timer;
            newHighScore = true;
        }
    }

    public void saveScores()
    {
        PlayerPrefs.SetFloat("maxTimer", maxTimer);
        PlayerPrefs.SetFloat("maxHigh", maxHigh);
        PlayerPrefs.SetFloat("maxDeep", maxDeep);
        PlayerPrefs.SetFloat("maxDelta", maxDelta);
        PlayerPrefs.SetInt("maxJump", maxJump);
        PlayerPrefs.SetInt("maxDive", maxDive);
        PlayerPrefs.SetInt("maxSmash", maxSmash);
        PlayerPrefs.SetInt("maxSlide", maxSlide);
        PlayerPrefs.SetInt("maxKills", maxKills);
        newHighScore = false;
    }
}
