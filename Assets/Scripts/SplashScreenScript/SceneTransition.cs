using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] RectTransform fader;
    [SerializeField] private Image fading;

   

    private void Start () {
        
        
       fader.gameObject.SetActive (true);

       // ALPHA
       LeanTween.alpha (fader, 1, 0);
       LeanTween.alpha (fader, 0, 0.5f).setOnComplete (() => {
           fader.gameObject.SetActive (false);
       });
       
      
    
            // SCALE
            /*LeanTween.scale (fader, new Vector3 (1, 1, 1), 0);
            LeanTween.scale (fader, Vector3.zero, 0.5f).setEase (LeanTweenType.easeInOutQuad).setOnComplete (() => {
                fader.gameObject.SetActive (false);
            });*/
            
            
    }


    public void TransitionScene(string sceneName)
    {
        
        fader.gameObject.SetActive (true);
    
           
        // ALPHA
             
        LeanTween.alpha (fader, 0, 0);
        LeanTween.alpha (fader, 1, 0.5f).setOnComplete (() => {
            
            SceneManager.LoadScene (sceneName);
            
        });
        
    }





    private  void OpenMenuScene () {
            
        
        fader.gameObject.SetActive (true);
    
           
        // ALPHA
             
        LeanTween.alpha (fader, 0, 0);
        LeanTween.alpha (fader, 1, 0.5f).setOnComplete (() => {
            
            SceneManager.LoadScene (0);
            
        });
    
            // SCALE
            /*LeanTween.scale (fader, Vector3.zero, 0f);
            LeanTween.scale (fader, new Vector3 (1, 1, 1), 0.5f).setEase (LeanTweenType.easeInOutQuad).setOnComplete (() => {
                SceneManager.LoadScene (0);
            });*/
        }
    
        
    private void OpenGameScene () {
            fader.gameObject.SetActive (true);
    
            // ALPHA
            LeanTween.alpha (fader, 0, 0);
            LeanTween.alpha (fader, 1, 0.5f).setOnComplete (() => {
            // Example for little pause before laoding the next scene
            Invoke ("LoadGame", 0.5f);
            });
    
            // SCALE
            /*LeanTween.scale (fader, Vector3.zero, 0f);
            LeanTween.scale (fader, new Vector3 (1, 1, 1), 0.5f).setEase (LeanTweenType.easeInOutQuad).setOnComplete (() => {
                // Example for little pause before laoding the next scene
                Invoke ("LoadGame", 0.5f);
            });*/
        }
    
       
    private void LoadGame () {
            SceneManager.LoadScene (1);
        }
}
