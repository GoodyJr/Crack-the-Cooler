using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float x = transform.position.x;
    float y = transform.position.y;
    float time = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)){
            time = 0;
            while (time <= 1){
                transform.position = new Vector3(0, y, 0);
                y += Time.deltaTime;
                time += Time.deltaTime / 100;
            }
        }
    }
}
