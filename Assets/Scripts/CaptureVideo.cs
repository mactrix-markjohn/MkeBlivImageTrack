using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using UnityEngine.UI;

public class CaptureVideo : MonoBehaviour
{
    public RawImage rawimage;
    private WebCamTexture webcamTexture;
    
    private MP4Recorder recorder;
    private Coroutine recordVideoCoroutine;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Obtain camera devices available
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        //Set a camera to the webcamTexture
        webcamTexture = new WebCamTexture(cam_devices[0].name, 480, 640, 30);
        //Set the webcamTexture to the texture of the rawimage
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        //Start the camera
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private IEnumerator SaveImage()
    {
        //Create a Texture2D with the size of the rendered image on the screen.
        Texture2D texture = new Texture2D(rawimage.texture.width, rawimage.texture.height, TextureFormat.ARGB32, false);
        //Save the image to the Texture2D
        texture.SetPixels(webcamTexture.GetPixels());
        texture = RotateTexture(texture, -90);
        texture.Apply();
        yield return new WaitForEndOfFrame();
        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(texture, "CameraTest", "CaptureImage.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));
        // To avoid memory leaks
        Destroy(texture);
    }
    
    public async void startRecording()
    {
        // Create a recorder
        recorder = new MP4Recorder(640, 480, 30, 6000000, keyframeInterval: 3); // bits per second
        //Start recording
        recordVideoCoroutine = StartCoroutine(recording());
    }
    
    public void clickCapture()
    {
        StartCoroutine(SaveImage());
    }
    
    private IEnumerator recording()
    {
        // Create a clock for generating recording timestamps
        var clock = new RealtimeClock();
        for (int i = 0; ; i++)
        {
            // Commit the frame to NatCorder for encoding
            recorder.CommitFrame(webcamTexture.GetPixels32(), clock.timestamp);
            // Wait till end of frame
            yield return new WaitForEndOfFrame();
        }
    }
    
    public async void stopRecording()
    {
        //Stop Coroutine
        StopCoroutine(recordVideoCoroutine);
        // Finish writing
        var recordingPath = await recorder.FinishWriting();
        //save video to gallery
        NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(recordingPath, "CameraTest", "testVideo.mp4", (success, path) => Debug.Log("Media save result: " + success + " " + path));
    }
    
    
    Texture2D RotateTexture (Texture2D texture, float eulerAngles)
    {
        int x;
        int y;
        int i;
        int j;
        float phi = eulerAngles / (180 / Mathf.PI);
        float sn = Mathf.Sin (phi);
        float cs = Mathf.Cos (phi);
        Color32[] arr = texture.GetPixels32 ();
        Color32[] arr2 = new Color32[arr.Length];
        int W = texture.width;
        int H = texture.height;
        int xc = W / 2;
        int yc = H / 2;
		
        for (j=0; j<H; j++) {
            for (i=0; i<W; i++) {
                arr2 [j * W + i] = new Color32 (0, 0, 0, 0);
				
                x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
                y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);
				
                if ((x > -1) && (x < W) && (y > -1) && (y < H)) { 
                    arr2 [j * W + i] = arr [y * W + x];
                }
            }
        }
		
        Texture2D newImg = new Texture2D (W, H);
        newImg.SetPixels32 (arr2);
        newImg.Apply ();
		
        return newImg;
    }
    
    
}
