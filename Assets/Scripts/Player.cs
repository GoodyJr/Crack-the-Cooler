using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 position;
    float time = 0;
    float speed = 2;
    float gridSize = 1;
    bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        position = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            time += Time.deltaTime;
            if (time >= gridSize / speed) {
                rb.velocity = new Vector2(0, 0);
                transform.position = position;
                position = new Vector2(transform.position.x, transform.position.y);
                moving = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W)) {
            rb.velocity = new Vector2(0, speed);
            position += new Vector2(0, gridSize);
            time = 0;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            rb.velocity = new Vector2(-1*speed, 0);
            position += new Vector2(-1*gridSize, 0);
            time = 0;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            rb.velocity = new Vector2(0, -1*speed);
            position += new Vector2(0, -1*gridSize);
            time = 0;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            rb.velocity = new Vector2(speed, 0);
            position += new Vector2(gridSize, 0);
            time = 0;
            moving = true;
        }
    }
}
