using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    public Button[] btns;
    public Sprite[] btnSprites;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("Level", 1);
        var a = PlayerPrefs.GetInt("Level");
        Debug.Log(a);
        for (int i = 0; i < btns.Length; i++)
        {
            //add listener to all buttons
            btns[i].onClick.AddListener(() => { LoadLevel(); });
            if (i < a - 1)
            {
                btns[i].image.sprite = btnSprites[0];
            }
            else if (i == a - 1)
            {
                btns[i].image.sprite = btnSprites[1];
            }
            else
            {
                btns[i].image.sprite = btnSprites[2];
                btns[i].interactable = false;
            }
        }

    }
    void LoadLevel()
    {
        var a = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(a);
    }
}
