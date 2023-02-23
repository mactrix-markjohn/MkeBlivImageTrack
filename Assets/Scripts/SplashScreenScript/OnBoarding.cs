using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OnBoarding : MonoBehaviour
{

    [SerializeField] private List<GameObject> Slides;
    private int currentNo = 0;
    private int prevNo = 0;
    private int listCount = 0;

    private void Awake()
    {
        //DeactivateURPRenderPipeline();
    }

    // Start is called before the first frame update
    void Start()
    {
        listCount = Slides.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SkipClick()
    {
        
        Debug.Log("Skip click is working");
        currentNo = listCount - 1;
        
        GameObject slide = Slides[currentNo];
        GameObject prevSlide = Slides[prevNo];

        prevNo = currentNo;

        prevSlide.GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f);

        slide.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);

    }

    public void NextClick()
    {
        Debug.Log("Next click is working");
        
        if (currentNo >= listCount)
        {
            currentNo = 0;
            return;
        }
            
        
        currentNo += 1;
        
        GameObject slide = Slides[currentNo];
        GameObject prevSlide = Slides[prevNo];

        prevNo = currentNo;

        prevSlide.GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f);

        slide.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);
       
    }

    public void GetStartedClick()
    {
       //TODO: Do some checking here with the firebase database and authentication
       GetComponent<SceneTransition>().TransitionScene(StringStore.EmailEntryScene);
       
    }
    
    void DeactivateURPRenderPipeline()
    {
        GraphicsSettings.renderPipelineAsset = null;
        QualitySettings.renderPipeline = null;
    }

    


}
