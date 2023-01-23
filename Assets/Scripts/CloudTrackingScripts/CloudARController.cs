using System;
using System.Collections;
using System.Collections.Generic;
/*using Firebase.Extensions;
using Firebase.Storage;*/
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CloudARController : MonoBehaviour
{
    
    
     
    /*FirebaseStorage storage;
    StorageReference storageReference;*/
    
    
    [FormerlySerializedAs("m_ImageManager")]
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager trackedImageManager;
    
    
    public XRReferenceImageLibrary xrReferenceImageLibrary;
    private RuntimeReferenceImageLibrary runtimeReferenceImageLibrary;
        
    

    // List of AR Spawn Object

    public GameObject[] ARObjectPrefabs;
    Dictionary<string,GameObject> prefabObjects = new Dictionary<string, GameObject>();
    
    Dictionary<string, GameObject> activeObjects = new Dictionary<string, GameObject>();

    private string prefabName;


    private void Awake()
    {
        
        
        foreach (var prefab in ARObjectPrefabs)
        {
            prefabObjects.Add(prefab.name,prefab);
        }
    }

    private void Start()
    {
        
        //TestVideoURL();
        //initialize storage reference
        //storage = FirebaseStorage.DefaultInstance;
        
        runtimeReferenceImageLibrary = trackedImageManager.CreateRuntimeLibrary(xrReferenceImageLibrary);
        trackedImageManager.referenceLibrary = runtimeReferenceImageLibrary;
        trackedImageManager.enabled = true;
       
        StartCoroutine(LoadImage(StringStore.StanbicReferenceImageLink, "StanbicCard"));
        
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

           // cube.GetComponent<MeshRenderer>().material.mainTexture = texture;
            //add to ReferenceImageLibrary
            StartCoroutine(AddImageToReferenceLibrary(texture,texturename));
            
            
        }
    }


    IEnumerator AddImageToReferenceLibrary(Texture2D imageToAdd, string texturename)
    {
        
        yield return null;

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
                    _ShowAndroidToastMessage(status);
                }

                if (jobstate.status.IsComplete())
                {
                    _ShowAndroidToastMessage("Job is complete : "+texturename);
                }

            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            
        }
    }

    void OnEnable()
    {

        trackedImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }
    
    
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // added, spawn prefab
        foreach(ARTrackedImage image in obj.added)
        {
            AddedObject(image);
        }
        
        // updated, set prefab position and rotation
        foreach(ARTrackedImage image in obj.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (image.trackingState == TrackingState.Tracking)
            {
                UpdatedObject(image,true);
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            else
            {
                UpdatedObject(image,false);
            }
        }
        
        // removed, destroy spawned instance
        foreach(ARTrackedImage image in obj.removed)
        {
            RemovedObject(image);
        }
    }
    
    
     void AddedObject(ARTrackedImage image)
    {

        GameObject prefab = ARObjectPrefabs[0];

        /*string imageName = image.referenceImage.name;
        string storagename = imageName.Remove(imageName.LastIndexOf('.'));*/
        
        GameObject arObject = Instantiate(prefab, image.transform.position, image.transform.rotation);
        arObject.name = prefab.name;
        
        arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,
            image.referenceImage.size.y);
        
        
        _ShowAndroidToastMessage("Video is about to play");
             
        arObject.GetComponent<VideoTrackedPrefabScript>().Setup(StringStore.StanbicVideoLink);
        
       
        // Retrieve the video from Firebase Storage
        
        /*storageReference = storage.GetReferenceFromUrl(StringStore.FSBucketLink)
            .Child(StringStore.FSTrackedContent).Child(storagename).Child(storagename+".mp4");
        
        storageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if(!task.IsFaulted && !task.IsCanceled)
            {
                string url = Convert.ToString(task.Result); 
                //Fetch file from the link
                
                //Play video
                arObject.GetComponent<VideoTrackedPrefabScript>().Setup(url);

            }
            else{
                Debug.Log(task.Exception);
            }
        }); */

        // add object to active objects dictionary
        prefabName = arObject.name;
        activeObjects.Add(arObject.name,arObject);
        
        
        
       
    }

    void UpdatedObject(ARTrackedImage image, bool isTracking)
    {
        
        GameObject arObject = activeObjects[prefabName];

        if (arObject == null)
            return;
        
        if (isTracking)
        {
            arObject.SetActive(true);
           
            var imageTransform = image.transform;
            arObject.transform.SetPositionAndRotation(imageTransform.position,imageTransform.rotation);
            arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,image.referenceImage.size.y);

        }
        else
        {
            
            arObject.SetActive(false);
        }
        
    }


    void RemovedObject(ARTrackedImage image)
    {

        if (activeObjects[prefabName] != null)
        {
            Destroy(activeObjects[prefabName]);
            activeObjects.Remove(prefabName);
        }
    }



    void TestVideoURL()
    {
        GameObject prefab = ARObjectPrefabs[0];

        
        
        GameObject arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        arObject.name = prefab.name;
        
       
       
        // Retrieve the video from Firebase Storage
        
        /*storageReference = storage.GetReferenceFromUrl(StringStore.FSBucketLink)
            .Child(StringStore.FSTrackedContent).Child(StringStore.StanbicCard).Child(StringStore.StanbicCard+".mp4");
        
        storageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if(!task.IsFaulted && !task.IsCanceled)
            {
                string url = Convert.ToString(task.Result); 
                //Fetch file from the link
                
                //Play video
                arObject.GetComponent<VideoTrackedPrefabScript>().Setup(url);

            }
            else{
                Debug.Log(task.Exception);
            }
        }); */
    }
    
    
    public static void _ShowAndroidToastMessage(string message)
    {


        if (Application.platform != RuntimePlatform.Android)
            return;

            AndroidJavaClass unityPlayer =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>(
                    "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }


}
