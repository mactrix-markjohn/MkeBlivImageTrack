using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailEntryController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetStartedButtonClick()
    {
        //TODO: Do some checking here with the firebase database and authentication
        PlayerPrefs.SetInt(StringStore.OnBoarded,5);
        GetComponent<SceneTransition>().TransitionScene(StringStore.ARLensScene);
    }
}
