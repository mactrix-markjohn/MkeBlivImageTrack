using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARLensManagerScript : MonoBehaviour
{
    
    [SerializeField]
    [Header("Image manager on the AR Session Origin")]
    ARTrackedImageManager m_ImageManager;

    [SerializeField]
    [Header("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;
    
    
    //Runtime Reference Image Library field
    private RuntimeReferenceImageLibrary runtimeReferenceImageLibrary;
    
    
    // List of Gameobject for Local AR 
    [Space]
    [Header("This Prefabs will be used for the Local AR experience")]
    public GameObject[] ARObjectPrefabs;
    Dictionary<string,GameObject> prefabObjects = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> activeObjects = new Dictionary<string, GameObject>();
    
    // List of Gameobject for Video 360 AR
    [Space]
    [Header("This Prefabs will be used for the 360 Video AR experience")]
    public GameObject[] Video360ARObjectPrefabs;
    Dictionary<string,GameObject> video360PrefabObjects = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> video360ActiveObjects = new Dictionary<string, GameObject>();
    
    
    
    // List of Gameobject for Cloud AR
    [Space]
    [Header("This Prefabs will be used for Cloud based AR experience")]
    public GameObject[] CloudARObjectPrefabs;
    Dictionary<string,GameObject> cloudPrefabObjects = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> cloudActiveObjects = new Dictionary<string, GameObject>();
    private string cloudPrefabName;
    
    
    // List of Gameobject for Cloud 3D Model AR
    private GameObject downloaded3DModel;  // Gameobject to store the downloaded 3D mode from the Cloud;
    
    // Get the Render pipeline asset, so we can easy switch it off. This will allow us view the 3D model
    // downloaded from the cloud
    
    public RenderPipelineAsset renderPipelineAsset;
    
    
    
    private void Awake()
    {
        
        // Add all Local Prefabs to the Local Dictionary
        foreach (var prefab in ARObjectPrefabs)
        {
            prefabObjects.Add(prefab.name,prefab);
        }
        
        
        // Add all Cloud Prefabs to the Cloud Dictionary
        foreach (var prefab in CloudARObjectPrefabs)
        {
            cloudPrefabObjects.Add(prefab.name,prefab);
        }
        
        // Add all 360 Video Prefabs to the 360 Video Dictionary
        foreach (var prefab in Video360ARObjectPrefabs)
        {
            video360PrefabObjects.Add(prefab.name,prefab);
        }
        
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        // This code convert the Reference Image Library to a Runtime Reference Image Library, so that Image can be added in runtime.
        runtimeReferenceImageLibrary = m_ImageManager.CreateRuntimeLibrary(m_ImageLibrary);
        m_ImageManager.referenceLibrary = runtimeReferenceImageLibrary;
        m_ImageManager.enabled = true;
        
        
        
        // Here we simply Fetch all the reference images from  the cloud
        // and then import them into the Runtime Image Library
        StartCoroutine(LoadImage(StringStore.BasketballImageWhite, "BasketballWhiteModel"));

        
        
        // This statement is a Stud command to load images from the cloud into the Runtime Reference Image Library
        StartCoroutine(LoadImage(StringStore.StanbicReferenceImageLink, StringStore.StanbicCardCloud));

       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Cloud AR function to Fetch image from server and add to Runtime Reference Image Library

    // Load the Images from a Cloud using its Url
    IEnumerator LoadImage(string MediaUrl, string texturename){

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl); //Create a request
        yield return request.SendWebRequest(); //Wait for the request to complete
        
        if((request.result == UnityWebRequest.Result.ProtocolError) || (request.result == UnityWebRequest.Result.ConnectionError)){
           
            Debug.Log(request.error);
        }
        else{
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture; 
            
            //add to ReferenceImageLibrary
            StartCoroutine(AddImageToReferenceLibrary(texture,texturename));
            
            
        }
    }
    
    // Add Image to the Runtime Reference Image Library
    IEnumerator AddImageToReferenceLibrary(Texture2D imageToAdd, string texturename)
    {
        yield return null;
        try
        {
            if (m_ImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
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

        
    #endregion
    
    
    void OnEnable()
    {
        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }
    
    
    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }
    
    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // ----- Added, spawn prefab ------------
        foreach(ARTrackedImage image in obj.added)
        {
            AddedObject(image);
        }
        
        
        // ------ Updated, set prefab position and rotation --------
        foreach(ARTrackedImage image in obj.updated)
        {
           
            // image is tracking or tracking with limited state,
            // show visuals and update it's position and rotation
            
            if (image.trackingState == TrackingState.Tracking)
            {
                UpdatedObject(image,true);
                
            }
            else
            {
                // image is no longer tracking,
                // disable visuals TrackingState.Limited TrackingState.None
                
                UpdatedObject(image,false);
                
            }
        }
        
        
        // ------ Removed, destroy spawned instance -------
        foreach(ARTrackedImage image in obj.removed)
        {
            RemovedObject(image);
        }
    }
    
    
    void AddedObject(ARTrackedImage image)
    {

        string imageName = image.referenceImage.name;

        if (imageName.Contains("Cloud"))
        {
            // if the image name ends with Cloud
            CloudAddedImageEvent(image);
            
            
        }else if (imageName.Contains("Video360"))
        {
           // if the image name ends with Video360 
           Video360AddedImageEvent(image);
           

        }else if (imageName.Contains("Model"))
        {
            
            ModelAddedImageEvent(image);

        }else
        {
            // if the Image name ends with Local
            LocalAddedImageEvent(image);
            
        }
    }
    
    
    void UpdatedObject(ARTrackedImage image, bool isTracking)
    {
        
        string imageName = image.referenceImage.name;

        if (imageName.Contains("Cloud"))
        {
            // if the image name ends with Cloud
            CloudUpdateImageEvent(image,isTracking);
            
            
        }else if (imageName.Contains("Video360"))
        {
            // if the image name ends with Video360 
            Video360UpdateImageEvent(image,isTracking);
            
            
        }else if (imageName.Contains("Model"))
        {
            ModelUpdatedImageEvent(image, isTracking);

        }else
        {
            // if the Image name ends with Local
            LocalUpdatedImageEvent(image,isTracking);
            
            
        }
    }
    
    
    void RemovedObject(ARTrackedImage image)
    {
        
        string imageName = image.referenceImage.name;

        if (imageName.Contains("Cloud"))
        {
            // if the image name ends with Cloud
            CloudRemoveImageEvent(image);
            
            
        }else if (imageName.Contains("Video360"))
        {
            // if the image name ends with Video360 
            Video360RemoveImageEvent(image);
            
            
        }else if (imageName.Contains("Model"))
        {
            ModelRemovedImageEvent(image);

        }else
        {
            //TODO: working on this 
            LocalRemoveImageEvent(image);
            
        }
    }





    #region Local Image Events Functions

    
    // ------- Local AR experince functions for AR Image change events ---------
    void LocalAddedImageEvent(ARTrackedImage image)
    {
        GameObject prefab = prefabObjects[image.referenceImage.name];

        GameObject arObject = Instantiate(prefab, image.transform.position, image.transform.rotation);
        arObject.name = prefab.name;
        
        arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,image.referenceImage.size.y);
        
        // add object to active objects dictionary
        activeObjects.Add(arObject.name,arObject);
    }

    void LocalUpdatedImageEvent(ARTrackedImage image, bool isTracking)
    {
        GameObject arObject = activeObjects[image.referenceImage.name];
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

    void LocalRemoveImageEvent(ARTrackedImage image)
    {
        if (activeObjects[image.referenceImage.name] != null)
        {
            Destroy(activeObjects[image.referenceImage.name]);
            activeObjects.Remove(image.referenceImage.name);
        }
    }
    

    #endregion

    
    #region Video 360 Image Events Functions

    // ------- Video 360 AR experience function for AR Image change events ------
    void Video360AddedImageEvent(ARTrackedImage image)
    {
        GameObject prefab = video360PrefabObjects[image.referenceImage.name];

        GameObject arObject = Instantiate(prefab, image.transform.position, image.transform.rotation);
        arObject.name = prefab.name;
        
        arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,image.referenceImage.size.y);
        
        // add object to active objects dictionary
        video360ActiveObjects.Add(arObject.name,arObject);
    }

    void Video360UpdateImageEvent(ARTrackedImage image, bool isTracking)
    {
        
        GameObject arObject = video360ActiveObjects[image.referenceImage.name];

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

    void Video360RemoveImageEvent(ARTrackedImage image)
    {
        if (video360ActiveObjects[image.referenceImage.name] != null)
        {
            Destroy(video360ActiveObjects[image.referenceImage.name]);
            video360ActiveObjects.Remove(image.referenceImage.name);
        }
    }


    #endregion
    
    #region Cloud Image Events Functions

    // ------- CLoud AR experience function for AR Image change events ------
    void CloudAddedImageEvent(ARTrackedImage image)
    {
        //TODO: Retrieving the Prefab should be by name, if there are multiple of them. But for now we will use this
        GameObject prefab = CloudARObjectPrefabs[0];

        GameObject arObject = Instantiate(prefab, image.transform.position, image.transform.rotation);
        arObject.name = prefab.name;
        
        arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,
            image.referenceImage.size.y);
            
        _ShowAndroidToastMessage("Video is about to play");
             
        //TODO: Please note that is this a temporary code, we will dynamically fetch the link from the cloud.
        arObject.GetComponent<VideoTrackedPrefabScript>().Setup(StringStore.StanbicVideoLink);
            
        // add object to active objects dictionary
        cloudPrefabName = arObject.name;
        cloudActiveObjects.Add(arObject.name,arObject);
    }

    void CloudUpdateImageEvent(ARTrackedImage image, bool isTracking)
    {
        GameObject arObject = cloudActiveObjects[cloudPrefabName];

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

    void CloudRemoveImageEvent(ARTrackedImage image)
    {
        if (cloudActiveObjects[cloudPrefabName] != null)
        {
            Destroy(cloudActiveObjects[cloudPrefabName]);
            cloudActiveObjects.Remove(cloudPrefabName);
        }
    }


    #endregion


    #region 3D Model Image Events Functions

    
    private void ModelAddedImageEvent(ARTrackedImage image)
    {
        //downloaded3DModel = Instantiate(prefabObjects["ARVRAfrica"], image.transform.position, image.transform.rotation);
        //downloaded3DModel.transform.localScale = new Vector3(image.referenceImage.size.x,downloaded3DModel.transform.localScale.y,image.referenceImage.size.y);

        StartCoroutine(Fetch3DModel(image));
       
        
        
    } 
    
    IEnumerator Fetch3DModel(ARTrackedImage image)
    {
        // First, deactivate the URP render pipeline, this will allow downloaded 3D model to be visible
        //DeactivateURPRenderPipeline();
        
        yield return new WaitForSeconds(2f);
        
        Fetch3DModelFromCloud fetch3DModelFromCloud = new Fetch3DModelFromCloud();
        //fetch3DModelFromCloud.StartFetchingModel(StringStore.Basketballfbx,"fbx");
        fetch3DModelFromCloud.StartFetchingModel(StringStore.Basketballfbx);

        yield return new WaitUntil(() => fetch3DModelFromCloud.isLoaded);

        downloaded3DModel = fetch3DModelFromCloud.Loaded3DModel;

        var imageTransform = image.transform;
        downloaded3DModel.transform.position = imageTransform.position;
        downloaded3DModel.transform.rotation = imageTransform.rotation;
        
        downloaded3DModel.transform.localScale = new Vector3(1f,1f,1f);
        
        
        
    }
    private void ModelUpdatedImageEvent(ARTrackedImage image, bool isTracking)
    {
        if (downloaded3DModel == null)
            return;
        
        
        if (isTracking)
        {
            downloaded3DModel.SetActive(true);
           
            var imageTransform = image.transform;
            //downloaded3DModel.transform.SetPositionAndRotation(imageTransform.position,imageTransform.rotation);
            downloaded3DModel.transform.position = imageTransform.position;
            downloaded3DModel.transform.rotation = imageTransform.rotation;
            
            
            //downloaded3DModel.transform.localScale = new Vector3(image.referenceImage.size.x,downloaded3DModel.transform.localScale.y,image.referenceImage.size.y);
            
            
        }
        else
        {
            
            downloaded3DModel.SetActive(false);
        }
    } 
    
    private void ModelRemovedImageEvent(ARTrackedImage image)
    {
        if (downloaded3DModel!= null)
        {
            Destroy(downloaded3DModel);
            
        }
        
        // Reactivate the URP Render pipeline, so that experience that depend on it, will work.
        //ActivateURPRenderPipeline();
    }
    

    #endregion
   



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

    void DeactivateURPRenderPipeline()
    {
        GraphicsSettings.renderPipelineAsset = null;
        QualitySettings.renderPipeline = null;
    }

    void ActivateURPRenderPipeline()
    {
        GraphicsSettings.renderPipelineAsset = renderPipelineAsset;
        QualitySettings.renderPipeline = renderPipelineAsset;
    }

}
