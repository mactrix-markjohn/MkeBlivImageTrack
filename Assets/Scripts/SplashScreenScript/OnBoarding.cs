using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

public class OnBoarding : MonoBehaviour
{

    [SerializeField] private List<GameObject> Slides;
    private int currentNo = 0;
    private int prevNo = 0;
    private int listCount = 0;

    [Space] 
    public GameObject arSession;
    public GameObject arSessionOrgin;
    

    private void Awake()
    {
        //arSession.SetActive(false);
        //arSessionOrgin.SetActive(false);
        //DeactivateURPRenderPipeline();

        int onBoadingCheck = PlayerPrefs.GetInt(StringStore.OnBoarded);
        if (onBoadingCheck == 5)
        {
            GetComponent<SceneTransition>().TransitionScene(StringStore.ARLensScene);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        listCount = Slides.Count;
        for (int i = 1; i < Slides.Capacity; i++)
        {
            Slides[i].SetActive(false);
        }
        
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
        
       

        prevSlide.GetComponent<CanvasGroup>().LeanAlpha(0, 1f).setOnComplete(() =>
        {
            prevSlide.SetActive(false);
        });;
        
        slide.SetActive(true);
        

        slide.GetComponent<CanvasGroup>().LeanAlpha(1, 1f);

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

      
        
        
        prevSlide.GetComponent<CanvasGroup>().LeanAlpha(0, 1f).setOnComplete(() =>
        {
            prevSlide.SetActive(false);
        });
        
        slide.SetActive(true);
        
        //check if it is slide 2
        if (currentNo == 1)
        {
            Permission.RequestUserPermission(Permission.Camera);
            
            arSession.SetActive(true);
            arSessionOrgin.SetActive(true);
            
            
            
        }

        slide.GetComponent<CanvasGroup>().LeanAlpha(1, 1f);
       
    }

    public void GetStartedClick()
    {
       //TODO: Do some checking here with the firebase database and authentication
       //GetComponent<SceneTransition>().TransitionScene(StringStore.ARLensScene);
       GetComponent<SceneTransition>().TransitionScene(StringStore.ImageTargetAssets);
       
       
       
    }
    
    void DeactivateURPRenderPipeline()
    {
        GraphicsSettings.renderPipelineAsset = null;
        QualitySettings.renderPipeline = null;
    }

    


}
