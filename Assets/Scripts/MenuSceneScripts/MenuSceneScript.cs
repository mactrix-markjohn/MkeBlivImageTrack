using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImageTrackingSceneClick()
    {
        SceneManager.LoadScene(StringStore.ImageTrackingHelperScene);
    }

    public void CloudImageTrackingClick()
    {
        SceneManager.LoadScene(StringStore.CloudImageTrackingScene);
    }

    public void Three60VideoClick()
    {
        SceneManager.LoadScene(StringStore.Video360Scene);
    }

    public void MoreClick()
    {
        
    }
}
