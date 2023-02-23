using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.URP.Mappers;
using UnityEngine;

public class Fetch3DModelFromCloud
{

    public bool isLoaded = false;  // Use to check if the 3D model is fully loaded from the cloud
    public GameObject Loaded3DModel = null;
    
    
    
    // Creates the AssetLoaderOptions instance, configures the Web Request, and downloads the Model.
    public void StartFetchingModel(string url, string fileExtension = "fbx")
    {
        if (string.IsNullOrEmpty(url))
            return;


        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        var webRequest = AssetDownloader.CreateWebRequest(url);
        AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions,null,fileExtension);
    }
    
    // Variation of StartFetchingModel but without fileExtension Argument. We will detect the file extension
    // from the URL provided by the user.
    public void StartFetchingModel(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;
        
        string fileExtension = "fbx";

        
        // Checks the file extention of the URL and assignment TriLib
        if (url.Contains(".fbx",StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = "fbx";

        }else if (url.Contains(".glb",StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = "glb";

        }else if (url.Contains(".zip",StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = "zip";

        }else if (url.Contains(".gltf",StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = "gltf";
            
        }else if (url.Contains(".obj",StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = "obj";
        }



        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        var webRequest = AssetDownloader.CreateWebRequest(url);
        AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions,null,fileExtension);
    }
    
    

    // Called when any error occurs.
    /// <param name="obj">The contextualized error, containing the original exception and
    /// the context passed to the method where the error was thrown.</param>
    private void OnError(IContextualizedError obj)
    {
        Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
    }

   
    // Called when the Model loading progress changes.
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    /// <param name="progress">The loading progress.</param>
    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        Debug.Log($"Loading Model. Progress: {progress:P}");
    }

    
    // Called when the Model (including Textures and Materials) has been fully loaded.
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");

        



    }

   
    /// Called when the Model Meshes and hierarchy are loaded.
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Model loaded. Loading materials.");
        isLoaded = true;
        Loaded3DModel = assetLoaderContext.RootGameObject;

    }
    
    
}
