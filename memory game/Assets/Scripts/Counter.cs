using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Counter : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float time = 10f;
    public static Counter Instance;
    //public bool count = false;
    public GameObject gameOverPanel;
    public int xv;
    bool load;

    private void Awake()
    {
        time = 60f;
        xv = 0;
        load = true;
        Instance = this;
        text.gameObject.SetActive(false);
    }
    void Update()
    {
        if (time > 0)
        {
            text.text = time.ToString("0.00");
            time -= (Time.deltaTime) - xv;
        }
        else
        {
            text.text = "";
            gameOverPanel.SetActive(true);
            var heartCount = PlayerPrefs.GetInt("Heart");
            //if (heartCount - 1 > 0)
            //{
            //    heartCount--;
            //    PlayerPrefs.SetInt("Heart", heartCount);
            //}
            if(load && heartCount == 1)
            {
                AdsManager.Instance.LoadRewardedAd();
                load = false;
                //reklam izletilecek
                //PlayerPrefs.SetInt("Heart", heartCount);
            }
            else if(load)
            {
                heartCount--;
                PlayerPrefs.SetInt("Heart", heartCount);
                load = false;
            }
            Debug.Log("Game Finished");
        }

    }
    public void ShowTimeText()
    {
        if(text.gameObject != null) text.gameObject.SetActive(true);
    }
}
