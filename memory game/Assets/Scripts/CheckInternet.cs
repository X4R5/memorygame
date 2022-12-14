using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInternet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            SceneManager.LoadScene("NoInternet");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
