/*
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FillRuntimeReferenceLibrary : MonoBehaviour
{
    
    
    FirebaseStorage storage;
    StorageReference storageReference;
    public ARTrackedImageManager trackedImageManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
       
        
        //initialize storage reference
        storage = FirebaseStorage.DefaultInstance;

        StorageReference initialRef = storage.GetReferenceFromUrl(StringStore.FSBucketLink);
        
        storageReference =  initialRef.Child(StringStore.FSReferenceImages).Child(StringStore.StanbicCardImage);
        
        storageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if(!task.IsFaulted && !task.IsCanceled){
                
                StartCoroutine(LoadImage(Convert.ToString(task.Result),storageReference.Name)); 
                //Fetch file from the link
            }
            else{
                Debug.Log(task.Exception);
            }
        }); 

       
    }
    
    IEnumerator LoadImage(string MediaUrl, string texturename){
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl); //Create a request
        yield return request.SendWebRequest(); //Wait for the request to complete
        
        if((request.result == UnityWebRequest.Result.ProtocolError) || (request.result == UnityWebRequest.Result.ConnectionError)){
           
            Debug.Log(request.error);
        }
        else{
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture; 
            // setting the loaded image to our object
            
            
            //add to ReferenceImageLibrary
            StartCoroutine(AddImageToReferenceLibrary(texture,texturename));
            
            
        }
    }


    IEnumerator AddImageToReferenceLibrary(Texture2D imageToAdd, string texturename)
    {

        try
        {
            
            if (trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                var jobstate = mutableLibrary.ScheduleAddImageWithValidationJob(
                    imageToAdd,
                    texturename,
                    0.1f);

                while (!jobstate.jobHandle.IsCompleted)
                {
                    string status = "Job is Running...";
                }

            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            
        }

        yield return null;

    }


    void UselessCode()
    {
    
        // StartCoroutine(LoadImage("https://firebasestorage.googleapis.com/v0/b/vikings-login.appspot.com/o/VikingsLogo.jpeg?alt=media&token=44b39032-34fe-489e-bd69-536f0ed7c251"));
        //Easy hardcoded solution But bad approach
    
    
        //get reference of image
        StorageReference image = storageReference.Child("VikingsLogo.jpeg");

        //Get the download link of file
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if(!task.IsFaulted && !task.IsCanceled){
               // StartCoroutine(LoadImage(Convert.ToString(task.Result))); //Fetch file from the link
            }
            else{
                Debug.Log(task.Exception);
            }
        }); 
    }
}
*/
