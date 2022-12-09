using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCards : MonoBehaviour
{
    [SerializeField] Transform puzzleField, preLevelPanel;
    [SerializeField] int cardCount = 2, difference = 1;
    [SerializeField] GameObject buttonPref, equalSign, textPref;
    List<GameObject> btns = new List<GameObject>();
    [SerializeField] TMP_Text timeRemainingText;
    private void Awake()
    {
        for (int i = 0; i < cardCount; i++)
        {
            var btn = Instantiate(buttonPref);
            btn.name = "" + i;
            btn.transform.SetParent(puzzleField, false);
            btns.Add(btn);
        }

        
    }
    private void Start()
    {
        Time.timeScale = 0f;
        timeRemainingText.text = "";
        GameController.Instance.PreGame();
        //Counter.Instance.IsCount(false);
        foreach (var btn in btns)
        {
            btn.SetActive(false);
        }

        if(difference != 0)
        {
            SetDifferences();
        }
        else
        {
            Play();
        }
        
    }
    
    void SetDifferences()
    {
        if (difference == 0) return;
        for (int i = 0; i < difference; i++)
        {
            var btn = Instantiate(buttonPref);
            btn.transform.SetParent(preLevelPanel, false);
            btn.tag = "Untagged";
            var equalsign = Instantiate(equalSign);
            equalSign.tag = "Untagged";
            equalsign.transform.SetParent(preLevelPanel, false);
            equalSign.GetComponent<Button>().image = null;
            var btn2 = Instantiate(buttonPref);
            btn2.transform.SetParent(preLevelPanel, false);
            btn2.tag = "Untagged";
            //btn.GetComponent<Button>().interactable = false;
            var a = GameController.Instance.RandomMatch();
            btn.GetComponent<Button>().image.sprite = a.Item1;
            if (a.Item2 != null)
            {
                btn2.GetComponent<Button>().image.sprite = a.Item2;
            }
            else
            {
                btn2.GetComponent<Button>().image.sprite = null;
                var textObj = Instantiate(textPref);
                textObj.transform.SetParent(btn2.transform, false);
                textObj.transform.localScale = btn2.transform.localScale;
                var text = textObj.GetComponent<TextMeshProUGUI>();
                var number = GameController.Instance.randomNumbers[i];
                Debug.Log("ac " + number);
                text.text = number.ToString();
                text.gameObject.SetActive(true);
                text.autoSizeTextContainer = false;
                text.fontSize = 100;
            }
        }
    }
    public void Play()
    {
        //Counter.Instance.IsCount(true);
        Time.timeScale = 1f;
        preLevelPanel.parent.gameObject.SetActive(false);
        foreach (var btn in btns)
        {
            btn.SetActive(true);
        }
        GameController.Instance.StartGame();
        Counter.Instance.ShowTimeText();
    }
}
