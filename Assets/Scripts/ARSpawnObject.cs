using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ARSpawnObject : MonoBehaviour
{

    public string name;
    public VideoPlayer videoPlayer;
    public Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onAdded()
    {
        if (videoPlayer != null)
        {
            //videoPlayer.Stop();
            videoPlayer.Play();
        }

        if (animator != null)
        {
           // animator.enabled = false;

            animator.enabled = true;
            animator.Play(0);

        }

    }

    public void onTracking()
    {
        if (videoPlayer != null)
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }
        }
    }

    public void onNotTracking()
    {
        if (videoPlayer != null)
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
        }
    }

    public void onRemoved()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
        
        if (animator != null)
        {
            animator.enabled = false;

        }
    }

    public void EnableObject(bool enable)
    {
        gameObject.SetActive(enable);
    }


}
