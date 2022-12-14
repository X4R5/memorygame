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

    private void Start()
    {
        //time = 60f;
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
            if(load && PlayerPrefs.GetInt("Heart") < 2)
            {
                AdsManager.Instance.LoadRewardedAd(true);
                GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " go";
                load = false;
                //reklam izletilecek
                //PlayerPrefs.SetInt("Heart", heartCount);
            }
            else if(load)
            {
                GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " go";
                PlayerPrefs.SetInt("Heart", PlayerPrefs.GetInt("Heart") - 1);
                AdsManager.Instance.LoadRewardedAd(false);
                load = false;
            }
            load = false;
            Debug.Log("Game Finished");
        }

    }
    public void ShowTimeText()
    {
        if(text.gameObject != null) text.gameObject.SetActive(true);
    }
}
