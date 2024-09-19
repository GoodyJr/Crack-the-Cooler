using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScript : MonoBehaviour
{
    public BoxCollider2D Collider;
    public GameObject Wren;
    public Player Player;
    public bool youWin = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = Wren.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Wren)
        {
            youWin = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == Wren && Player.moving == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

    }
}
