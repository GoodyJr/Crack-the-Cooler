using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public GameObject dialog;
    public Tilemap map;
    public TileBase wall0;
    public TileBase wall1;
    public TileBase wall2;
    public TileBase wall3;
    public TileBase trapdoor;
    public TileBase slippery;
    public TileBase letter0;
    public TileBase letter1;
    public TileBase letter2;
    public TileBase letter3;
    public GameObject deathTimer;
    public CountdownTimer countdownTimer;
    public GameObject Victory;
    public VictoryScript VictoryScript;
    Rigidbody2D rb;
    Vector2 position;
    Vector2 start;
    TileBase up;
    TileBase left;
    TileBase down;
    TileBase right;
    ArrayList wall = new ArrayList();
    ArrayList letter = new ArrayList();
    float time = 0.0f;
    float speed = 3.0f;
    float gridSize = 1.0f;
    float koyote = 0.1f;
    public bool moving = false;
    int x;
    int y;
    int direction;
    bool first = false;
    bool next = false;
    bool nextDown = false;
    bool upNext = false;
    bool leftNext = false;
    bool downNext = false;
    bool rightNext = false;
    int directionNext = 0;
    int firstLetter;
    int secondLetter;
    int thirdLetter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        countdownTimer = deathTimer.GetComponent<CountdownTimer>();
        VictoryScript = Victory.GetComponent<VictoryScript>();
        wall.Add(wall0);
        wall.Add(wall1);
        wall.Add(wall2);
        wall.Add(wall3);
        letter.Add(letter0);
        letter.Add(letter1);
        letter.Add(letter2);
        letter.Add(letter3);
        position = new Vector2(transform.position.x, transform.position.y);
        start = position;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        up = map.GetTile(new Vector3Int(x, y + 1, 0));
        left = map.GetTile(new Vector3Int(x - 1, y, 0));
        down = map.GetTile(new Vector3Int(x, y - 1, 0));
        right = map.GetTile(new Vector3Int(x + 1, y, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            time += Time.deltaTime;
            first = false;
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
                if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && directionNext == 1) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) && directionNext == 2) || (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow) && directionNext == 3) || (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow) && directionNext == 4)) {
                    directionNext = 0;
                }
                nextDown = false;
                if (map.GetTile(new Vector3Int(x, y, 0)) == trapdoor) {
                    Death();
                }
                else if (map.GetTile(new Vector3Int(x, y, 0)) == slippery) {
                    if (direction == 1 && !wall.Contains(up)) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(0, speed);
                        position += new Vector2(0, gridSize);
                    }
                    if (direction == 2 && !wall.Contains(left)) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(-1*speed, 0);
                        position += new Vector2(-1*gridSize, 0);
                    }
                    if (direction == 3 && !wall.Contains(down)) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(0, -1*speed);
                        position += new Vector2(0, -1*gridSize);
                    }
                    if (direction == 4 && !wall.Contains(right)) {
                        moving = true;
                        time = 0;
                        rb.velocity = new Vector2(speed, 0);
                        position += new Vector2(gridSize, 0);
                    }
                }
                else if (letter.Contains(map.GetTile(new Vector3Int(x, y, 0)))) {
                    firstLetter = Random.Range(1, 27);
                    secondLetter = Random.Range(1, 27);
                    thirdLetter = Random.Range(1, 27);
                }
            }
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || upNext) && !wall.Contains(up) && !dialog.activeSelf && !VictoryScript.youWin) {
            rb.velocity = new Vector2(0, speed);
            position += new Vector2(0, gridSize);
            time = 0;
            moving = true;
            direction = 1;
            first = true;
            next = false;
            upNext = false;
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || leftNext) && !wall.Contains(left) && !dialog.activeSelf && !VictoryScript.youWin) {
            rb.velocity = new Vector2(-1*speed, 0);
            position += new Vector2(-1*gridSize, 0);
            time = 0;
            moving = true;
            direction = 2;
            first = true;
            next = false;
            leftNext = false;
            transform.localScale = new Vector2(-0.5f, 0.5f);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || downNext) && !wall.Contains(down) && !dialog.activeSelf && !VictoryScript.youWin) {
            rb.velocity = new Vector2(0, -1*speed);
            position += new Vector2(0, -1*gridSize);
            time = 0;
            moving = true;
            direction = 3;
            first = true;
            next = false;
            downNext = false;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || rightNext) && !wall.Contains(right) && !dialog.activeSelf && !VictoryScript.youWin) {
            rb.velocity = new Vector2(speed, 0);
            position += new Vector2(gridSize, 0);
            time = 0;
            moving = true;
            direction = 4;
            first = true;
            next = false;
            rightNext = false;
            transform.localScale = new Vector2(0.5f, 0.5f);
        }
        if (!first && !next && !dialog.activeSelf) {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                next = true;
                upNext = true;
                leftNext = false;
                downNext = false;
                rightNext = false;
                directionNext = 1;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                next = true;
                upNext = false;
                leftNext = true;
                downNext = false;
                rightNext = false;
                directionNext = 2;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                next = true;
                upNext = false;
                leftNext = false;
                downNext = true;
                rightNext = false;
                directionNext = 3;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                next = true;
                upNext = false;
                leftNext = false;
                downNext = false;
                rightNext = true;
                directionNext = 4;
            }
            if (!nextDown) {
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 1)) {
                    nextDown = true;
                    upNext = true;
                    leftNext = false;
                    downNext = false;
                    rightNext = false;
                    directionNext = 1;
                }
                else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 2)) {
                    nextDown = true;
                    upNext = false;
                    leftNext = true;
                    downNext = false;
                    rightNext = false;
                    directionNext = 2;
                }
                else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 3)) {
                    nextDown = true;
                    upNext = false;
                    leftNext = false;
                    downNext = true;
                    rightNext = false;
                    directionNext = 3;
                }
                else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 4)) {
                    nextDown = true;
                    upNext = false;
                    leftNext = false;
                    downNext = false;
                    rightNext = true;
                    directionNext = 4;
                }
            }
        }
        if (countdownTimer.TimeRemaining <= 0) {
            Death();
        }
    }

    public void Death() {
        transform.position = start;
        position = start;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        up = map.GetTile(new Vector3Int(x, y + 1, 0));
        left = map.GetTile(new Vector3Int(x - 1, y, 0));
        down = map.GetTile(new Vector3Int(x, y - 1, 0));
        right = map.GetTile(new Vector3Int(x + 1, y, 0));
        transform.localScale = new Vector2(0.5f, 0.5f);
        countdownTimer.TimeRemaining = countdownTimer.MaxTime;
        countdownTimer.SecondCounter = 0;
    }
}
