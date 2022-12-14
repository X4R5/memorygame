using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject soundButton;
    [SerializeField] Sprite[] soundSprites;
    [SerializeField] Sprite greenHeartSprite;
    private void Awake()
    {
        if(PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
    }
    private void Start()
    {
        
        soundButton = GameObject.Find("SoundButton");
        var a = PlayerPrefs.GetInt("PlaySound") == 1 ? 1 : 0;
        if(SceneManager.GetActiveScene().name != "Levels") soundButton.GetComponent<Button>().image.sprite = soundSprites[a];
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            var gs = GameObject.FindGameObjectsWithTag("Heart");
            var b = PlayerPrefs.GetInt("Heart");
            for (int i = 0; i < b; i++)
            {
                Debug.Log("b = " + b);
                gs[i].GetComponent<Image>().sprite = greenHeartSprite;
            }
        }
        //Debug.Log(a);
    }
    public void PlaySounds()
    {
        var a = PlayerPrefs.GetInt("PlaySound") == 1 ? 0 : 1;
        PlayerPrefs.SetInt("PlaySound", a);
        soundButton.GetComponent<Button>().image.sprite = soundSprites[a];
        //Debug.Log(a);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene("Menu");
    }
    public void NextLevel()
    {
        //var a = PlayerPrefs.GetInt("Level");
        var a = SceneManager.GetActiveScene().name;
        var b = int.Parse(a);
        SceneManager.LoadScene((b + 1).ToString());

    }

    public void Levels()
    {
        SceneManager.LoadScene("Levels");
    }
    
    public void PlayMenu()
    {
        var a = PlayerPrefs.GetInt("Level");
        
        SceneManager.LoadScene((a).ToString());
    }

    public void Quit()
    {
        Application.Quit();
    }
}
