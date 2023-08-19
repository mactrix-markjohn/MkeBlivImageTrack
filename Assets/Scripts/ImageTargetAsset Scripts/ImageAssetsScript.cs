using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageAssetsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownloadImageTarget()
    {
        
        Application.OpenURL("https://drive.google.com/drive/folders/1-huPn_g5h78ztzw3rkrebfxmb2_Iyy-Q");
        
    }

    public void GetStartedButtonClick()
    {
        //TODO: Do some checking here with the firebase database and authentication
        
        GetComponent<SceneTransition>().TransitionScene(StringStore.ARLensScene);
    }
}
