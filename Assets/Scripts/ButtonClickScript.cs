using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenu()
    {
        //SceneManager.LoadScene(StringStore.MenuScene);
        SceneManager.LoadScene("TestCaptureVideoARLensScene");
        //Application.Quit();
    }
    
    
}
