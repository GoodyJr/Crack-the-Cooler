using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    public GameObject wren;
    Player player;

    public void pause()
    {
        Debug.Log("button pressed");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Start() {
        player = wren.GetComponent<Player>();
    }

    //Debug Stuff
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !player.letterOn)
        {
            Debug.Log("activate");
            pause();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("YouClicked!");
        }
    }
}
