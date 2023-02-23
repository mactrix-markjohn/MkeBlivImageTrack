using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangePipeline : MonoBehaviour
{
    public RenderPipelineAsset exampleAssetA;
    public RenderPipelineAsset exampleAssetB;


    private void Awake()
    {
        GraphicsSettings.renderPipelineAsset = null;
        QualitySettings.renderPipeline = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            GraphicsSettings.renderPipelineAsset = exampleAssetA;
            Debug.Log("Default render pipeline asset is: " + GraphicsSettings.renderPipelineAsset.name);
        }
        else if (Input.GetKeyDown(KeyCode.B)) {
            GraphicsSettings.renderPipelineAsset = exampleAssetB;
            Debug.Log("Default render pipeline asset is: " + GraphicsSettings.renderPipelineAsset.name);
        }
    }
}
