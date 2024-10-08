using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text Text;
    public int MaxTime;
    public int TimeRemaining;
    public float SecondCounter;
    public GameObject Dialogue;

    public GameObject victory;
    public VictoryScript VictoryScript;

    // Start is called before the first frame update
    void Start()
    {
        TimeRemaining = MaxTime;
        Text.text = TimeRemaining.ToString();

        VictoryScript = victory.GetComponent<VictoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(VictoryScript.youWin == true)
        {
            gameObject.SetActive(false);
        }

        if (Dialogue.activeSelf == false)
        {
            SecondCounter += Time.deltaTime;
        }

        if (SecondCounter >= 1)
        {
            SecondCounter = 0;
            TimeRemaining -= 1;
        }
        Text.text = TimeRemaining.ToString();
    }
}
