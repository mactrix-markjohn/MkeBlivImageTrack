using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoundTrackScript : MonoBehaviour
{
    
    
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager m_ImageManager;

    /// <summary>
    /// Get the <c>ARTrackedImageManager</c>
    /// </summary>
    public ARTrackedImageManager ImageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;

    /// <summary>
    /// Get the <c>XRReferenceImageLibrary</c>
    /// </summary>
    public XRReferenceImageLibrary ImageLibrary
    {
        get => m_ImageLibrary;
        set => m_ImageLibrary = value;
    }
    
    
    // List of AR Spawn Object

    public GameObject[] ARObjectPrefabs;
    Dictionary<string,GameObject> prefabObjects = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> activeObjects = new Dictionary<string, GameObject>();
   // private GameObject arObject;
    


    private void Awake()
    {
        foreach (var prefab in ARObjectPrefabs)
        {
            
            prefabObjects.Add(prefab.name,prefab);
            
            // -------- Useless code ----------------
            /*GameObject spawned = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            spawned.name = prefab.name;
            prefabObjects.Add(spawned.name,spawned);
            spawned.SetActive(false);*/
        }
    }

    private void Start()
    {
        m_ImageManager.referenceLibrary = m_ImageLibrary;
        m_ImageManager.enabled = true;
    }

    void OnEnable()
    {
       // s_FirstImageGUID = m_ImageLibrary[0].guid;
        
        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }
    
    
    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
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

        GameObject prefab = prefabObjects[image.referenceImage.name];

        GameObject arObject = Instantiate(prefab, image.transform.position, image.transform.rotation);
        arObject.name = prefab.name;
        
        arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,image.referenceImage.size.y);
        
        // add object to active objects dictionary
        activeObjects.Add(arObject.name,arObject);
        
        
        // -------- Useless code ---------------------
        /*GameObject arObject = prefabObjects[image.referenceImage.name];
        arObject.transform.SetPositionAndRotation(image.transform.position,image.transform.rotation);
        arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,image.referenceImage.size.y);
        
        arObject.SetActive(true);
       // arObject.GetComponent<ARSpawnObject>().onAdded();
        
        foreach (GameObject go in prefabObjects.Values)
        {
            if (go.name != image.referenceImage.name)
                go.SetActive(false);
            
        }*/
    }

    void UpdatedObject(ARTrackedImage image, bool isTracking)
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
        
        
        
        
        /*GameObject arObject = prefabObjects[image.referenceImage.name];
        
        if (isTracking)
        {
            
            arObject.transform.SetPositionAndRotation(image.transform.position,image.transform.rotation);
            arObject.transform.localScale = new Vector3(image.referenceImage.size.x,arObject.transform.localScale.y,image.referenceImage.size.y);
        
            arObject.SetActive(true);
            //arObject.GetComponent<ARSpawnObject>().onTracking();
           

        }
        else
        {
            //arObject.GetComponent<ARSpawnObject>().onNotTracking();
            arObject.SetActive(false);
        }
        
        foreach (GameObject go in prefabObjects.Values)
        {
            if (go.name != image.referenceImage.name)
                go.SetActive(false);
        }*/
    }


    void RemovedObject(ARTrackedImage image)
    {

        if (activeObjects[image.referenceImage.name] != null)
        {
            Destroy(activeObjects[image.referenceImage.name]);
            activeObjects.Remove(image.referenceImage.name);
        }



        /*GameObject arObject = prefabObjects[image.referenceImage.name];
        arObject.SetActive(false);*/
    }
    
}
