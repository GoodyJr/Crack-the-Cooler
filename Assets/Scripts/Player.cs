using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public GameObject dialog;
    public Tilemap map;
    public GameObject deathTimer;
    public CountdownTimer countdownTimer;
    public GameObject Victory;
    public VictoryScript VictoryScript;
    public bool moving = false;
    Rigidbody2D rb;
    TileBase up;
    TileBase left;
    TileBase down;
    TileBase right;
    Vector2 position;
    Vector2 start;
    float time = 0.0f;
    float speed = 3.0f;
    float gridSize = 1.0f;
    float koyote = 0.1f;
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

    public TileBase wall0;
    public TileBase wall1;
    public TileBase wall2;
    public TileBase wall3;
    ArrayList wall = new ArrayList();

    public TileBase trapdoor;

    public TileBase slippery;

    public TileBase letter0;
    public TileBase letter1;
    public TileBase letter2;
    public TileBase letter3;
    public GameObject letters;
    public TextMeshProUGUI letterOne;
    public TextMeshProUGUI letterTwo;
    public TextMeshProUGUI letterThree;
    public bool letterOn;
    ArrayList letter = new ArrayList();
    int firstLetter;
    int secondLetter;
    int thirdLetter;
    bool one;
    bool two;
    bool three;
    string upperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
    string lowerCase = "qwertyuiopasdfghjklzxcvbnm";
    bool letterUp = false;
    bool letterLeft = false;
    bool letterDown = false;
    bool letterRight = false;

    public TileBase encounter;
    public GameObject dodgeArrow;
    public GameObject dodgeCircle;
    ArrayList path = new ArrayList();
    Vector2 dodgeCircleSize = new Vector2(4, 4);
    bool encounterOn = false;
    float dodgeTimer = 0.0f;
    int dodge;
    float dodgeStart = 0.9f;
    float dodgeStop = 1.1f;
    bool encounterUp = false;
    bool encounterLeft = false;
    bool encounterDown = false;
    bool encounterRight = false;

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
        letters.SetActive(false);
        dodgeArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (letterOn) {
            if (!one) {
                if (Input.inputString == letterOne.text || Input.inputString == lowerCase[firstLetter].ToString()) {
                    one = true;
                    letterOne.gameObject.SetActive(false);
                }
                else if (!string.IsNullOrEmpty(Input.inputString)) {
                    firstLetter = Random.Range(0, 26);
                    secondLetter = Random.Range(0, 26);
                    thirdLetter = Random.Range(0, 26);
                    letterOne.text = upperCase[firstLetter].ToString();
                    letterTwo.text = upperCase[secondLetter].ToString();
                    letterThree.text = upperCase[thirdLetter].ToString();
                }
            }
            else if (!two) {
                if (Input.inputString == letterTwo.text || Input.inputString == lowerCase[secondLetter].ToString()) {
                    two = true;
                    letterTwo.gameObject.SetActive(false);
                }
                else if (!string.IsNullOrEmpty(Input.inputString)) {
                    firstLetter = Random.Range(0, 26);
                    secondLetter = Random.Range(0, 26);
                    thirdLetter = Random.Range(0, 26);
                    letterOne.text = upperCase[firstLetter].ToString();
                    letterTwo.text = upperCase[secondLetter].ToString();
                    letterThree.text = upperCase[thirdLetter].ToString();
                    one = false;
                    letterOne.gameObject.SetActive(true);
                }
            }
            else if (!three) {
                if (Input.inputString == letterThree.text || Input.inputString == lowerCase[thirdLetter].ToString()) {
                    three = true;
                    if (letterThree.text == "W") {
                        letterUp = true;
                    }
                    else if (letterThree.text == "A") {
                        letterLeft = true;
                    }
                    else if (letterThree.text == "S") {
                        letterDown = true;
                    }
                    else if (letterThree.text == "D") {
                        letterRight = true;
                    }
                }
                else if (!string.IsNullOrEmpty(Input.inputString)) {
                    firstLetter = Random.Range(0, 26);
                    secondLetter = Random.Range(0, 26);
                    thirdLetter = Random.Range(0, 26);
                    letterOne.text = upperCase[firstLetter].ToString();
                    letterTwo.text = upperCase[secondLetter].ToString();
                    letterThree.text = upperCase[thirdLetter].ToString();
                    one = false;
                    two = false;
                    letterOne.gameObject.SetActive(true);
                    letterTwo.gameObject.SetActive(true);
                }
            }
            else {
                letters.SetActive(false);
                letterOne.gameObject.SetActive(true);
                letterTwo.gameObject.SetActive(true);
                letterOn = false;
            }
        }
        else {
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
                        firstLetter = Random.Range(0, 26);
                        secondLetter = Random.Range(0, 26);
                        thirdLetter = Random.Range(0, 26);
                        one = false;
                        two = false;
                        three = false;
                        letterOne.text = upperCase[firstLetter].ToString();
                        letterTwo.text = upperCase[secondLetter].ToString();
                        letterThree.text = upperCase[thirdLetter].ToString();
                        letters.SetActive(true);
                        letterOn = true;
                        next = false;
                        nextDown = false;
                        upNext = false;
                        leftNext = false;
                        downNext = false;
                        rightNext = false;
                    }
                    else if (map.GetTile(new Vector3Int(x, y, 0)) == encounter) {
                        time = 0;
                        encounterOn = true;
                        next = false;
                        nextDown = false;
                        upNext = false;
                        leftNext = false;
                        downNext = false;
                        rightNext = false;
                        dodgeTimer = 0;
                        dodge = Random.Range(1,5);
                        dodgeCircle.transform.localScale = dodgeCircleSize;
                        dodgeArrow.SetActive(true);
                    }
                }
            }
            else if (encounterOn) {
                dodgeTimer += Time.deltaTime;
                dodgeCircle.transform.localScale -= new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 0);
                if (dodge == 1) {
                    dodgeArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else if (dodge == 2) {
                    dodgeArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else if (dodge == 3) {
                    dodgeArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
                else if (dodge == 4) {
                    dodgeArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (dodgeTimer > dodgeStop) {
                    dodgeTimer = 0;
                    dodge = Random.Range(1,5);
                    dodgeCircle.transform.localScale = dodgeCircleSize;
                    if (down == trapdoor) {
                        direction = 1;
                    }
                    else if (right == trapdoor) {
                        direction = 2;
                    }
                    else if (up == trapdoor) {
                        direction = 3;
                    }
                    else if (left == trapdoor) {
                        direction = 4;
                    }
                    else {
                        direction = (int)path[path.Count - 1];
                        path.RemoveAt(path.Count - 1);
                    }
                    if (direction == 1) {
                        MoveDown();
                    }
                    if (direction == 2) {
                        transform.localScale = new Vector2(-0.5f, 0.5f);
                        MoveRight();
                    }
                    if (direction == 3) {
                        MoveUp();
                    }
                    if (direction == 4) {
                        transform.localScale = new Vector2(0.5f, 0.5f);
                        MoveLeft();
                    }
                }
                else if (dodgeTimer >= dodgeStart && ((dodge == 1 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) || (dodge == 2 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) || (dodge == 3 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) || (dodge == 4 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))))) {
                    dodgeTimer = 0;
                    encounterOn = false;
                    dodgeArrow.SetActive(false);
                    if (dodge == 1) {
                        encounterUp = true;
                    }
                    else if (dodge == 2) {
                        encounterLeft = true;
                    }
                    else if (dodge == 3) {
                        encounterDown = true;
                    }
                    else if (dodge == 4) {
                        encounterRight = true;
                    }
                }
            }
            else if (((Input.GetKeyDown(KeyCode.W) && !letterUp) || Input.GetKeyDown(KeyCode.UpArrow) || upNext) && !wall.Contains(up) && !dialog.activeSelf && !VictoryScript.youWin) {
                path.Add(1);
                MoveUp();
            }
            else if (((Input.GetKeyDown(KeyCode.A) && !letterLeft) || Input.GetKeyDown(KeyCode.LeftArrow) || leftNext) && !wall.Contains(left) && !dialog.activeSelf && !VictoryScript.youWin) {
                path.Add(2);
                transform.localScale = new Vector2(-0.5f, 0.5f);
                MoveLeft();
            }
            else if (((Input.GetKeyDown(KeyCode.S) && !letterDown) || Input.GetKeyDown(KeyCode.DownArrow) || downNext) && !wall.Contains(down) && !dialog.activeSelf && !VictoryScript.youWin) {
                path.Add(3);
                MoveDown();
            }
            else if (((Input.GetKeyDown(KeyCode.D) && !letterRight) || Input.GetKeyDown(KeyCode.RightArrow) || rightNext) && !wall.Contains(right) && !dialog.activeSelf && !VictoryScript.youWin) {
                path.Add(4);
                transform.localScale = new Vector2(0.5f, 0.5f);
                MoveRight();
            }
            if (!first && !next && !dialog.activeSelf && !letterOn && !encounterOn) {
                if ((Input.GetKeyDown(KeyCode.W) && !letterUp && !encounterUp) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    next = true;
                    upNext = true;
                    leftNext = false;
                    downNext = false;
                    rightNext = false;
                    directionNext = 1;
                }
                else if ((Input.GetKeyDown(KeyCode.A) && !letterLeft && !encounterLeft) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    next = true;
                    upNext = false;
                    leftNext = true;
                    downNext = false;
                    rightNext = false;
                    directionNext = 2;
                }
                else if ((Input.GetKeyDown(KeyCode.S) && !letterDown && !encounterDown) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    next = true;
                    upNext = false;
                    leftNext = false;
                    downNext = true;
                    rightNext = false;
                    directionNext = 3;
                }
                else if ((Input.GetKeyDown(KeyCode.D) && !letterRight && !encounterRight) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    next = true;
                    upNext = false;
                    leftNext = false;
                    downNext = false;
                    rightNext = true;
                    directionNext = 4;
                }
                if (!nextDown) {
                    if (((Input.GetKey(KeyCode.W) && !letterUp && !encounterUp) || Input.GetKey(KeyCode.UpArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 1)) {
                        nextDown = true;
                        upNext = true;
                        leftNext = false;
                        downNext = false;
                        rightNext = false;
                        directionNext = 1;
                    }
                    else if (((Input.GetKey(KeyCode.A) && !letterLeft && !encounterLeft) || Input.GetKey(KeyCode.LeftArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 2)) {
                        nextDown = true;
                        upNext = false;
                        leftNext = true;
                        downNext = false;
                        rightNext = false;
                        directionNext = 2;
                    }
                    else if (((Input.GetKey(KeyCode.S) && !letterDown && !encounterDown) || Input.GetKey(KeyCode.DownArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 3)) {
                        nextDown = true;
                        upNext = false;
                        leftNext = false;
                        downNext = true;
                        rightNext = false;
                        directionNext = 3;
                    }
                    else if (((Input.GetKey(KeyCode.D) && !letterRight && !encounterRight) || Input.GetKey(KeyCode.RightArrow)) && time >= gridSize * (1 - koyote) / speed && (directionNext == 0 || directionNext == 4)) {
                        nextDown = true;
                        upNext = false;
                        leftNext = false;
                        downNext = false;
                        rightNext = true;
                        directionNext = 4;
                    }
                }
            }
        }
        if (letterUp && Input.GetKeyUp(KeyCode.W)) {
            letterUp = false;
        }
        else if (letterLeft && Input.GetKeyUp(KeyCode.A)) {
            letterLeft = false;
        }
        else if (letterDown && Input.GetKeyUp(KeyCode.S)) {
            letterDown = false;
        }
        else if (letterRight && Input.GetKeyUp(KeyCode.D)) {
            letterRight = false;
        }
        else if (encounterUp && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))) {
            letterUp = false;
        }
        else if (encounterLeft && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))) {
            letterLeft = false;
        }
        else if (encounterDown && (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))) {
            letterDown = false;
        }
        else if (encounterRight && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))) {
            letterRight = false;
        }
        if (countdownTimer.TimeRemaining <= 0) {
            Death();
        }
        //lift animation
        if(VictoryScript.liftTime > 0)
        {
            transform.position += new Vector3(0, 2 * Time.deltaTime, 0);
        }

    }

    void MoveUp() {
        rb.velocity = new Vector2(0, speed);
        position += new Vector2(0, gridSize);
        time = 0;
        moving = true;
        direction = 1;
        first = true;
        next = false;
        upNext = false;
    }

    void MoveLeft() {
        rb.velocity = new Vector2(-1*speed, 0);
        position += new Vector2(-1*gridSize, 0);
        time = 0;
        moving = true;
        direction = 2;
        first = true;
        next = false;
        leftNext = false;
    }

    void MoveDown() {
        rb.velocity = new Vector2(0, -1*speed);
        position += new Vector2(0, -1*gridSize);
        time = 0;
        moving = true;
        direction = 3;
        first = true;
        next = false;
        downNext = false;
    }

    void MoveRight() {
        rb.velocity = new Vector2(speed, 0);
        position += new Vector2(gridSize, 0);
        time = 0;
        moving = true;
        direction = 4;
        first = true;
        next = false;
        rightNext = false;
    }

    void Death() {
        letters.SetActive(false);
        letterOne.gameObject.SetActive(true);
        letterTwo.gameObject.SetActive(true);
        letterOn = false;
        dodgeArrow.SetActive(false);
        dodgeTimer = 0;
        encounterOn = false;
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
