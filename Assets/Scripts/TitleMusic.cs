using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMusic : MonoBehaviour
{
    public static TitleMusic Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level") && !SceneManager.GetActiveScene().name.Contains("Select"))
        {
            Destroy(gameObject);
        }
    }
}
