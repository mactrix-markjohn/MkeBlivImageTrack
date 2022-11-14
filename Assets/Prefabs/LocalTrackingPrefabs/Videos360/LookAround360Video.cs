using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround360Video : MonoBehaviour
{

    public float speed = 3;
    public GameObject sphere360Video;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.GetTouch(0);
        
        if (touch.phase == TouchPhase.Moved)
        {
            
            
        }
    }
}
