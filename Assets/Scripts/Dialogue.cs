using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textspeed;
    public string[] lines;
    public string[] profileImage;
    public UnityEngine.UI.Image image;
    public Sprite Wren;
    public Sprite Dusty;
    public GameObject profileObject;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) == true | Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }

    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //set character image
        if (int.Parse(profileImage[index]) == 1)
        {
            image.color = Color.white;
            image.sprite = Wren;
        }
        else if (int.Parse(profileImage[index]) == 2)
        {
            image.color = Color.white;
            image.sprite = Dusty;
        }
        else if (int.Parse(profileImage[index]) == 0)
        {
            image.color = new Color(255, 255, 255, 0);
        }

        //types lines
            foreach (char c in lines[index].ToCharArray())
        {
            profileObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(-10, 10));

            textComponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine (TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
