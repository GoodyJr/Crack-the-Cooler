using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 position;
    Vector2 start;
    public Tilemap map;
    public TileBase wall;
    public TileBase trapdoor;
    public TileBase slippery;
    TileBase up;
    TileBase left;
    TileBase down;
    TileBase right;
    float time = 0;
    float speed = 3;
    float gridSize = 1;
    bool moving = false;
    int x;
    int y;
    int direction;
    public GameObject DeathTimer;
    public CountdownTimer CountdownTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        position = new Vector2(transform.position.x, transform.position.y);
        start = position;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        up = map.GetTile(new Vector3Int(x, y + 1, 0));
        left = map.GetTile(new Vector3Int(x - 1, y, 0));
        down = map.GetTile(new Vector3Int(x, y - 1, 0));
        right = map.GetTile(new Vector3Int(x + 1, y, 0));
        CountdownTimer = DeathTimer.GetComponent<CountdownTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            time += Time.deltaTime;
            if (time >= gridSize / speed) {
                moving = false;
                rb.velocity = new Vector2(0, 0);
                transform.position = position;
                x = (int)transform.position.x;
                y = (int)transform.position.y;
                up = map.GetTile(new Vector3Int(x, y + 1, 0));
                left = map.GetTile(new Vector3Int(x - 1, y, 0));
                down = map.GetTile(new Vector3Int(x, y - 1, 0));
                right = map.GetTile(new Vector3Int(x + 1, y, 0));
                if (map.GetTile(new Vector3Int(x, y, 0)) == trapdoor) {
                    Death();
                }
                else if (map.GetTile(new Vector3Int(x, y, 0)) == slippery) {
                    if (direction == 1 && up != wall) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(0, speed);
                        position += new Vector2(0, gridSize);
                    }
                    if (direction == 2 && left != wall) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(-1*speed, 0);
                        position += new Vector2(-1*gridSize, 0);
                    }
                    if (direction == 3 && down != wall) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(0, -1*speed);
                        position += new Vector2(0, -1*gridSize);
                    }
                    if (direction == 4 && right != wall) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(speed, 0);
                        position += new Vector2(gridSize, 0);
                    }
                }
            }
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && up != wall) {
            rb.velocity = new Vector2(0, speed);
            position += new Vector2(0, gridSize);
            time = 0;
            moving = true;
            direction = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && left != wall) {
            rb.velocity = new Vector2(-1*speed, 0);
            position += new Vector2(-1*gridSize, 0);
            time = 0;
            moving = true;
            direction = 2;
            transform.localScale = new Vector2(-0.5f, 0.5f);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && down != wall) {
            rb.velocity = new Vector2(0, -1*speed);
            position += new Vector2(0, -1*gridSize);
            time = 0;
            moving = true;
            direction = 3;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && right != wall) {
            rb.velocity = new Vector2(speed, 0);
            position += new Vector2(gridSize, 0);
            time = 0;
            moving = true;
            direction = 4;
            transform.localScale = new Vector2(0.5f, 0.5f);
        }


        if (CountdownTimer.TimeRemaining <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        transform.position = start;
        position = start;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        up = map.GetTile(new Vector3Int(x, y + 1, 0));
        left = map.GetTile(new Vector3Int(x - 1, y, 0));
        down = map.GetTile(new Vector3Int(x, y - 1, 0));
        right = map.GetTile(new Vector3Int(x + 1, y, 0));
        CountdownTimer.TimeRemaining = CountdownTimer.MaxTime;
        CountdownTimer.SecondCounter = 0;
    }
}
