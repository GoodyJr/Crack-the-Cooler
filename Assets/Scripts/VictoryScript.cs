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
    public GameObject door;
    private bool victorySequence = false;
    public float liftTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        Player = Wren.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        if (victorySequence == true)
        {
            if (door.transform.position.y - transform.position.y > 0.15)
            {
                door.transform.position = door.transform.position - new Vector3(0, Time.deltaTime, 0);
            }
            else if(liftTime < 2)
            {
                transform.position += new Vector3(0, 2*Time.deltaTime, 0);
                liftTime += Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
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

        if (collision.gameObject == Wren && Player.moving == false && victorySequence == false)
        {
            victorySequence = true;
        }

    }
}
