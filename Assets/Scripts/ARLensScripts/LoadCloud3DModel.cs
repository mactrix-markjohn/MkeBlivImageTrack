using System;
using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using UnityEngine;

public class LoadCloud3DModel : MonoBehaviour
{
    public string URL;
    public string fileExtension;
    
    
        private void Start()
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                StartFetchingModel(URL);
            }
            else
            {
                StartFetchingModel(URL,fileExtension);
            }


           // var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            //var webRequest = AssetDownloader.CreateWebRequest("https://ricardoreis.net/trilib/demos/sample/TriLibSampleModel.zip");
            //AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
        }
        
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

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            Debug.Log($"Loading Model. Progress: {progress:P}");
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Materials loaded. Model fully loaded.");
            

        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Model loaded. Loading materials.");
        }
}
