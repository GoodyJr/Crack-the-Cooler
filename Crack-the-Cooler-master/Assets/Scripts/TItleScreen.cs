using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void StartPressed()
    {
        SceneManager.LoadScene("Level 0");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LevelSelectButton()
    {
        SceneManager.LoadScene("Level Select");
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
}
