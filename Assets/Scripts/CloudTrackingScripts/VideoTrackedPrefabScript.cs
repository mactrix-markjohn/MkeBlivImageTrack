using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTrackedPrefabScript : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Setup(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Prepare();
        videoPlayer.Play();

    }
}
