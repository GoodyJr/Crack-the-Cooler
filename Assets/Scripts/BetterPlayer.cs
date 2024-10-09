using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class BetterPlayer : MonoBehaviour
{
    public GameObject dialog;
    public Tilemap map;
    public GameObject deathTimer;
    public CountdownTimer countdownTimer;
    public GameObject Victory;
    public VictoryScript VictoryScript;
    public bool moving = false;
    Rigidbody2D rb;
    Animator anim;
    ArrayList directionOrder;
    TileBase up;
    TileBase left;
    TileBase down;
    TileBase right;
    Vector2 position;
    Vector2 start;
    float time = 0.0f;
    float speed = 3.0f;
    float gridSize = 1.0f;
    int x;
    int y;
    int direction;

    public TileBase wall0;
    public TileBase wall1;
    public TileBase wall2;
    public TileBase wall3;
    ArrayList wall = new ArrayList();

    public TileBase trapdoor;
    bool trap;
    bool trapAnim;

    public TileBase slippery;
    bool sliding;

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
    public TileBase encounterPressed;
    public GameObject dodgeArrow;
    public GameObject dodgeCircle;
    GameObject guard;
    GameObject guardChild;
    GameObject guardChildChild;
    Animator guardAnim;
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
    int currentDirection;
    int nextDirection;
    int modifier;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        guard = transform.GetChild(0).gameObject;
        guardChild = guard.transform.GetChild(0).gameObject;
        guardChildChild = guardChild.transform.GetChild(0).gameObject;
        guardAnim = guardChildChild.GetComponent<Animator>();
        countdownTimer = deathTimer.GetComponent<CountdownTimer>();
        VictoryScript = Victory.GetComponent<VictoryScript>();
        directionOrder.Add(0);
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
        guard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count > 0) {
            Debug.Log(path[path.Count - 1]);
        }
        anim.SetBool("Moving", moving);
        anim.SetBool("Trap", trap);
        anim.SetBool("Sliding", sliding);
        if (guard.activeSelf == true) {
            guardAnim.SetBool("Moving", moving);
            guardAnim.SetBool("Encounter", encounterOn);
        }
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
                if (encounterOn) {
                    guard.transform.Rotate(0, 0, 90 * Time.deltaTime * speed * modifier * guard.transform.localScale.x);
                    guardChild.transform.Rotate(0, 0, 90 * Time.deltaTime * speed * modifier * -1);
                }
                if (time >= gridSize / speed) {
                    moving = false;
                    sliding = false;
                    rb.velocity = new Vector2(0, 0);
                    transform.position = position;
                    x = (int)transform.position.x;
                    y = (int)transform.position.y;
                    up = map.GetTile(new Vector3Int(x, y + 1, 0));
                    left = map.GetTile(new Vector3Int(x - 1, y, 0));
                    down = map.GetTile(new Vector3Int(x, y - 1, 0));
                    right = map.GetTile(new Vector3Int(x + 1, y, 0));
                    if (map.GetTile(new Vector3Int(x, y, 0)) == trapdoor) {
                        trap = true;
                        StartCoroutine(Trapdoor());
                    }
                    else if (map.GetTile(new Vector3Int(x, y, 0)) == slippery && !encounterOn) {
                        sliding = true;
                        path.Add(path[path.Count - 1]);
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
                    else if (letter.Contains(map.GetTile(new Vector3Int(x, y, 0))) && !encounterOn) {
                        directionOrder.Clear();
                        directionOrder.Add(0);
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
                    }
                    else if (map.GetTile(new Vector3Int(x, y, 0)) == encounter) {
                        directionOrder.Clear();
                        directionOrder.Add(0);
                        map.SetTile(new Vector3Int(x, y, 0), encounterPressed);
                        time = 0;
                        encounterOn = true;
                        dodgeTimer = 0;
                        dodge = Random.Range(1,5);
                        dodgeCircle.transform.localScale = dodgeCircleSize;
                        if (direction == 1) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, 90);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (direction == 2) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, 180);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (direction == 3) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, -90);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (direction == 4) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, 0);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        guard.SetActive(true);
                        nextDirection = direction;
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
                        dodgeArrow.SetActive(true);
                    }
                }
            }
            else if (encounterOn && !trap) {
                if (dodgeTimer == 0) {
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
                }
                dodgeTimer += Time.deltaTime;
                dodgeCircle.transform.localScale -= new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 0);
                if (dodgeTimer > dodgeStop) {
                    dodgeTimer = 0;
                    dodge = Random.Range(1,5);
                    dodgeCircle.transform.localScale = dodgeCircleSize;
                    currentDirection = nextDirection;
                    if (down == trapdoor) {
                        nextDirection = 1;
                    }
                    else if (right == trapdoor) {
                        nextDirection = 2;
                    }
                    else if (up == trapdoor) {
                        nextDirection = 3;
                    }
                    else if (left == trapdoor) {
                        nextDirection = 4;
                    }
                    else {
                        nextDirection = (int)path[path.Count - 1];
                        path.RemoveAt(path.Count - 1);
                    }
                    if (currentDirection == 1 && nextDirection == 4) {
                        currentDirection = 5;
                    }
                    else if (currentDirection == 4 && nextDirection == 1) {
                        currentDirection = 0;
                    }
                    if (currentDirection == nextDirection) {
                        modifier = 0;
                    }
                    else if (currentDirection > nextDirection) {
                        modifier = -1;
                    }
                    else if (currentDirection < nextDirection) {
                        modifier = 1;
                    }
                    if (nextDirection == 1) {
                        MoveDown();
                    }
                    if (nextDirection == 2) {
                        transform.localScale = new Vector2(-0.5f, 0.5f);
                        guard.transform.localScale = new Vector2(-1, 1);
                        guardChild.transform.localScale = new Vector2(-1, 1);
                        if (currentDirection == 1 || currentDirection == 5) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, -90 * guard.transform.localScale.x);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (currentDirection == 3) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, 90 * guard.transform.localScale.x);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        MoveRight();
                    }
                    if (nextDirection == 3) {
                        MoveUp();
                    }
                    if (nextDirection == 4) {
                        transform.localScale = new Vector2(0.5f, 0.5f);
                        guard.transform.localScale = new Vector2(1, 1);
                        guardChild.transform.localScale = new Vector2(1, 1);
                        if (currentDirection == 1 || currentDirection == 5) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, 90 * guard.transform.localScale.x);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (currentDirection == 3) {
                            guard.transform.rotation = Quaternion.Euler(0, 0, -90 * guard.transform.localScale.x);
                            guardChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        MoveLeft();
                    }
                }
                else if (dodgeTimer >= dodgeStart && ((dodge == 1 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) || (dodge == 2 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) || (dodge == 3 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) || (dodge == 4 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))))) {
                    dodgeTimer = 0;
                    encounterOn = false;
                    dodgeArrow.SetActive(false);
                    guard.SetActive(false);
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
            else if (((Input.GetKeyDown(KeyCode.W) && !letterUp) || Input.GetKeyDown(KeyCode.UpArrow) || (int)directionOrder[directionOrder.Count - 1] == 1 || (int)directionOrder[directionOrder.Count - 1] == 5) && !wall.Contains(up) && !dialog.activeSelf && !VictoryScript.youWin && !trap && !letterOn && !encounterOn) {
                if (path.Count != 0 && (int)path[path.Count - 1] == 3) {
                    path.RemoveAt(path.Count - 1);
                }
                else {
                    path.Add(1);
                }
                MoveUp();
            }
            else if (((Input.GetKeyDown(KeyCode.A) && !letterLeft) || Input.GetKeyDown(KeyCode.LeftArrow) || (int)directionOrder[directionOrder.Count - 1] == 2 || (int)directionOrder[directionOrder.Count - 1] == 6) && !wall.Contains(left) && !dialog.activeSelf && !VictoryScript.youWin && !trap && !letterOn && !encounterOn) {
                if (path.Count != 0 && (int)path[path.Count - 1] == 4) {
                    path.RemoveAt(path.Count - 1);
                }
                else {
                    path.Add(2);
                }
                transform.localScale = new Vector2(-0.5f, 0.5f);
                guard.transform.localScale = new Vector2(-1, 1);
                guardChild.transform.localScale = new Vector2(-1, 1);
                MoveLeft();
            }
            else if (((Input.GetKeyDown(KeyCode.S) && !letterDown) || Input.GetKeyDown(KeyCode.DownArrow) || (int)directionOrder[directionOrder.Count - 1] == 3 || (int)directionOrder[directionOrder.Count - 1] == 7) && !wall.Contains(down) && !dialog.activeSelf && !VictoryScript.youWin && !trap && !letterOn && !encounterOn) {
                if (path.Count != 0 && (int)path[path.Count - 1] == 1) {
                    path.RemoveAt(path.Count - 1);
                }
                else {
                    path.Add(3);
                }
                MoveDown();
            }
            else if (((Input.GetKeyDown(KeyCode.D) && !letterRight) || Input.GetKeyDown(KeyCode.RightArrow) || (int)directionOrder[directionOrder.Count - 1] == 4 || (int)directionOrder[directionOrder.Count - 1] == 8) && !wall.Contains(right) && !dialog.activeSelf && !VictoryScript.youWin && !trap && !letterOn && !encounterOn) {
                if (path.Count != 0 && (int)path[path.Count - 1] == 2) {
                    path.RemoveAt(path.Count - 1);
                }
                else {
                    path.Add(4);
                }
                transform.localScale = new Vector2(0.5f, 0.5f);
                guard.transform.localScale = new Vector2(1, 1);
                guardChild.transform.localScale = new Vector2(1, 1);
                MoveRight();
            }
            if (!dialog.activeSelf && !trap && !letterOn && !encounterOn) {
                if (Input.GetKey(KeyCode.W) && !directionOrder.Contains(1)) {
                    directionOrder.Add(1);
                }
                if (Input.GetKey(KeyCode.A) && !directionOrder.Contains(2)) {
                    directionOrder.Add(2);
                }
                if (Input.GetKey(KeyCode.S) && !directionOrder.Contains(3)) {
                    directionOrder.Add(3);
                }
                if (Input.GetKey(KeyCode.D) && !directionOrder.Contains(4)) {
                    directionOrder.Add(4);
                }
                if (Input.GetKey(KeyCode.UpArrow) && !directionOrder.Contains(5)) {
                    directionOrder.Add(5);
                }
                if (Input.GetKey(KeyCode.LeftArrow) && !directionOrder.Contains(6)) {
                    directionOrder.Add(6);
                }
                if (Input.GetKey(KeyCode.DownArrow) && !directionOrder.Contains(7)) {
                    directionOrder.Add(7);
                }
                if (Input.GetKey(KeyCode.RightArrow) && !directionOrder.Contains(8)) {
                    directionOrder.Add(8);
                }
                if (Input.GetKeyUp(KeyCode.W) && directionOrder.Contains(1)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(1));
                }
                if (Input.GetKeyUp(KeyCode.A) && directionOrder.Contains(2)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(2));
                }
                if (Input.GetKeyUp(KeyCode.S) && directionOrder.Contains(3)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(3));
                }
                if (Input.GetKeyUp(KeyCode.D) && directionOrder.Contains(4)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(4));
                }
                if (Input.GetKeyUp(KeyCode.UpArrow) && directionOrder.Contains(5)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(5));
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow) && directionOrder.Contains(6)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(6));
                }
                if (Input.GetKeyUp(KeyCode.DownArrow) && directionOrder.Contains(7)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(7));
                }
                if (Input.GetKeyUp(KeyCode.RightArrow) && directionOrder.Contains(8)) {
                    directionOrder.RemoveAt(directionOrder.IndexOf(8));
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
        if (encounterUp && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))) {
            encounterUp = false;
        }
        else if (encounterLeft && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))) {
            encounterLeft = false;
        }
        else if (encounterDown && (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))) {
            encounterDown = false;
        }
        else if (encounterRight && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))) {
            encounterRight = false;
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
    }

    void MoveLeft() {
        rb.velocity = new Vector2(-1*speed, 0);
        position += new Vector2(-1*gridSize, 0);
        time = 0;
        moving = true;
        direction = 2;
    }

    void MoveDown() {
        rb.velocity = new Vector2(0, -1*speed);
        position += new Vector2(0, -1*gridSize);
        time = 0;
        moving = true;
        direction = 3;
    }

    void MoveRight() {
        rb.velocity = new Vector2(speed, 0);
        position += new Vector2(gridSize, 0);
        time = 0;
        moving = true;
        direction = 4;
    }

    void Death() {
        rb.velocity = new Vector2(0, 0);
        moving = false;
        directionOrder.Clear();
        directionOrder.Add(0);
        trap = false;
        letters.SetActive(false);
        letterOne.gameObject.SetActive(true);
        letterTwo.gameObject.SetActive(true);
        letterOn = false;
        dodgeArrow.SetActive(false);
        guard.SetActive(false);
        dodgeTimer = 0;
        encounterOn = false;
        map.SwapTile(encounterPressed, encounter);
        transform.position = start;
        position = start;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        up = map.GetTile(new Vector3Int(x, y + 1, 0));
        left = map.GetTile(new Vector3Int(x - 1, y, 0));
        down = map.GetTile(new Vector3Int(x, y - 1, 0));
        right = map.GetTile(new Vector3Int(x + 1, y, 0));
        transform.localScale = new Vector2(0.5f, 0.5f);
        guard.transform.localScale = new Vector2(1, 1);
        guardChild.transform.localScale = new Vector2(1, 1);
        countdownTimer.TimeRemaining = countdownTimer.MaxTime;
        countdownTimer.SecondCounter = 0;
        path.Clear();
    }

    IEnumerator Trapdoor() {
        yield return new WaitForSeconds(1);
        trap = false;
        Death();
    }
}
