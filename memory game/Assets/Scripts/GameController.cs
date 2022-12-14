using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.PlayerLoop;

public class GameController : MonoBehaviour
{
    public GameObject matchedText, grids, textPref;
    public List<Button> btns = new List<Button>();
    public Sprite bgImage;
    public Sprite[] puzzles, randomPuzzles;
    public List<Sprite> gamePuzzlesToShow = new List<Sprite>();
    public List<Sprite> gamePuzzlesToCheck = new List<Sprite>();
    public List<Sprite> differencesInPuzzles = new List<Sprite>();
    public List<Sprite> differencesInRandom = new List<Sprite>();
    public List<int> randomNumbers = new List<int>();
    public List<int> randomNumberIndexes = new List<int>();
    [SerializeField] GameObject preLevelPanel, levelClearedPanel;
    AudioSource audioMngr;
    public AudioClip click;
    float ctr;
    [SerializeField] bool useNumbersForRandomMatch;
    bool firstGuess, secondGuess, showMatchedText, canClick = true;
    int firstGuessIndex, secondGuessIndex, matchedCount;
    public static GameController Instance;


    public bool playSounds => PlayerPrefs.GetInt("PlaySound") == 1 ? true : false;
    private void Awake()
    {
        preLevelPanel.SetActive(true);
        audioMngr = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        Instance = this;
        puzzles = Resources.LoadAll<Sprite>(SceneManager.GetActiveScene().name);
        randomPuzzles = Resources.LoadAll<Sprite>(SceneManager.GetActiveScene().name + "Random");
    }
    
    
    public void PreGame()
    {
        GetButtons();
        AddGamePuzzles();
        CopyList();
    }
    public void StartGame()
    {
        ctr = 1f;
        AddListeners();
        StartCoroutine(RemoveGridLayout());
    }
    void CopyList()
    {
        foreach(var puzzle in gamePuzzlesToCheck)
        {
            gamePuzzlesToShow.Add(puzzle);
        }
    }

    public (Sprite, Sprite) RandomMatch()
    {
        //finding any puzzle
        var index = Random.Range(0, gamePuzzlesToCheck.Count);
        var a = gamePuzzlesToCheck[index];
        foreach(var rp in randomPuzzles)
        {
            var x = 0;
            foreach (var sp in differencesInPuzzles)
            {
                if (sp == a)
                {
                    x++;
                    break;
                }
            }
            if (x != 0)
            {
                index = Random.Range(0, gamePuzzlesToCheck.Count);
                a = gamePuzzlesToCheck[index];
            }
            else
            {
                differencesInPuzzles.Add(a);
                break;
            }
        }

        //finding random
        Sprite b = null;
        if (useNumbersForRandomMatch == false)
        {
            b = randomPuzzles[Random.Range(0, randomPuzzles.Length)];
            var ok2 = false;
            //b = randomPuzzles[Random.Range(0, randomPuzzles.Length)];
            while (ok2 == false)
            {
                var x = 0;
                foreach (var sp in differencesInRandom)
                {
                    if (sp == b)
                    {
                        x++;
                        break;
                    }
                }
                if (x != 0)
                {
                    b = randomPuzzles[Random.Range(0, randomPuzzles.Length)];
                }
                else
                {
                    differencesInRandom.Add(b);
                    ok2 = true;
                }
            }
            gamePuzzlesToShow[index] = b;
        }
        else
        {
            b = null;
            gamePuzzlesToShow[index] = b;
            var btn = btns[index];
            var textObj = Instantiate(textPref);
            textObj.transform.SetParent(btn.transform, false);
            textObj.transform.localScale = btn.transform.localScale;
            var text = textObj.GetComponent<TextMeshProUGUI>();
            var rand = Random.Range(1, 99);
            Debug.Log("gc " + rand);
            text.text = rand.ToString();
            text.autoSizeTextContainer = false;
            text.fontSize = 100;
            textObj.SetActive(false);
            randomNumbers.Add(rand);
            randomNumberIndexes.Add(index);
        }

        //Debug.Log(i);
        return (a, b);
    }

    IEnumerator RemoveGridLayout()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(grids.GetComponent<GridLayoutGroup>());
    }
    private void Update()
    {
        if (showMatchedText && ctr >= 0f)
        {
            matchedText.SetActive(true);
            ctr -= Time.deltaTime;
        }
        else
        {
            ctr = 1f;
            showMatchedText = false;
            matchedText.SetActive(false);
        }
    }
    
    void GetButtons() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        foreach (GameObject obj in objects)
        {
            btns.Add(obj.GetComponent<Button>());
            btns.Last().image.sprite = bgImage;
        }
    }
    void AddGamePuzzles()
    {
        int btncount = btns.Count;
        int index = Random.Range(0, puzzles.Length);
        var b = 0;
        for (int i = 0; i < btncount; i++)
        {
            if (b == btncount / 2)
            {
                var a = b;
                for (int j = 0; j < a; j++)
                {
                    var sprite = gamePuzzlesToCheck[j];
                    gamePuzzlesToCheck.Add(sprite);
                }
                break;
            }
            gamePuzzlesToCheck.Add(puzzles[index]);
            if(index != puzzles.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            b++;
        }
        
        Shuffle(gamePuzzlesToCheck);
    }
    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            var randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    void AddListeners()
    {
        foreach (var btn in btns)
        {
            btn.onClick.AddListener(() => Pick());
        }
    }
    void CheckIsGameFinished()
    {
        if (matchedCount == btns.Count / 2)
        {
            var a = PlayerPrefs.GetInt("Level");
            var x = SceneManager.GetActiveScene().name;
            var b = int.Parse(x);
            if (b >= a && b < 10)
            {
                PlayerPrefs.SetInt("Level", b + 1);
            }
            levelClearedPanel.SetActive(true);
            AdsManager.Instance.LoadRewardedAd(false);
            Counter.Instance.xv = 15;
        }
    }
    IEnumerator CheckIfThePuzzlesMatch()
    {
        canClick = false;
        yield return new WaitForSeconds(1f);
        if (gamePuzzlesToCheck[firstGuessIndex] == gamePuzzlesToCheck[secondGuessIndex])
        {
            Debug.Log("Match");
            showMatchedText = true;
            Destroy(btns[firstGuessIndex].gameObject);
            Destroy(btns[secondGuessIndex].gameObject);
            matchedCount++;
            CheckIsGameFinished();
        }
        if(gamePuzzlesToCheck[firstGuessIndex] != gamePuzzlesToCheck[secondGuessIndex])
        {
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
            firstGuess = false;
            secondGuess = false;
            CheckNumber(firstGuessIndex, false);
            CheckNumber(secondGuessIndex, false);
        }
        //yield return new WaitForSeconds(0.5f);
        firstGuess = false;
        secondGuess = false;
        canClick = true;
        //yield return new WaitForSeconds(0.5f);
    }

    void CheckNumber(int a, bool isactive)
    {
        if (!useNumbersForRandomMatch) return;
        foreach (var number in randomNumberIndexes)
        {
            if(number == a)
            {
                btns[a].transform.GetChild(0).gameObject.SetActive(isactive);
                break;
            }
        }
    }
    
    public void Pick()
    {   if (canClick == false) return;
        if (firstGuess == false)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            btns[firstGuessIndex].image.sprite = gamePuzzlesToShow[firstGuessIndex];
            if(playSounds) PlayClickSound();
            CheckNumber(firstGuessIndex, true);
        }
        else if (secondGuess == false && firstGuess)
        {
            secondGuess = true;
            if (playSounds) PlayClickSound();
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            if(secondGuessIndex == firstGuessIndex)
            {

                secondGuess = false;
                //firstGuess = false;
                //btns[firstGuessIndex].image.sprite = bgImage;
                //btns[secondGuessIndex].image.sprite = bgImage;
                //CheckNumber(secondGuessIndex, false);
                //CheckNumber(firstGuessIndex, false);

                return;
            }
            CheckNumber(secondGuessIndex, true);
            btns[secondGuessIndex].image.sprite = gamePuzzlesToShow[secondGuessIndex];
            StartCoroutine(CheckIfThePuzzlesMatch());
        }
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("You picked card number " + name);
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = gamePuzzlesToShow[int.Parse(name)];
    }

    private void PlayClickSound()
    {
        audioMngr.PlayOneShot(click);
    }
}
