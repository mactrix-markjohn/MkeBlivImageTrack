using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringStore
{
    
    // ------- Naming convention for Reference Images from the Cloud,
    // for 360 videos and local images ------------------------------
    public static string sampleCloudImage = "<NameOfImage>Cloud";
    public static string CloudImagesSuffix = "Cloud";
    public static string LocalImagesSuffix = "Local";
    public static string Video360ImagesSuffix = "Video360";
    
    
    
    // --------- Reference Image Library image names -----------------
    public static string ARVRAfrica = "ARVRAfrica";
    public static string StanbicCard = "StanbicCard";
    public static string CrayonPhoto = "CrayonPhoto";
    public static string Bikephoto = "Bikephoto";
    public static string Alphainvestmentphoto = "Alphainvestmentphoto";
    public static string Onekraizeenightphoto = "Onekraizeenightphoto";
    public static string SphereVideo = "SphereVideo";
    public static string StanbicCardCloud = "StanbicCardCloud";
    
    
    // --------- Firebase Storage folder names -----------------------
    public static string FSImages = "Images";
    public static string FSModels = "Models";
    public static string FSReferenceImages = "ReferenceImages";
    public static string FSVideos = "Videos";
    public static string FSVideo360 = "Video360";
    public static string FSTrackedContent = "TrackedContent";
    
    // --------- Firebase Storage buckek link ------------------------
    public static string FSBucketLink = "gs://mkebliv-4057f.appspot.com";
    public static string StanbicCardImage = "StanbicCard.png";
    
    // --------- Storage content links ----------
    public static string StanbicVideoLink =
        "https://firebasestorage.googleapis.com/v0/b/mkebliv-4057f.appspot.com/o/TrackedContent%2FStanbicCard%2FStanbicCard.mp4?alt=media&token=467de1d4-6000-44f2-a5e9-c659baddc883";

    /*
    public static string StanbicReferenceImageLink = 
        "https://firebasestorage.googleapis.com/v0/b/mkebliv-4057f.appspot.com/o/ReferenceImages%2FStanbicCard.png?alt=media&token=ac401a93-0108-49c6-989d-23dd8d5328ef";
        */


    public static string StanbicReferenceImageLink =
        "https://firebasestorage.googleapis.com/v0/b/mkebliv-4057f.appspot.com/o/ReferenceImages%2FStanbicCardclean.png?alt=media&token=eaa5c320-f65b-452a-805d-fb8f9d5e029d";
    
    // --------- Scenes ---------
    public static string MenuScene = "MenuScene";
    public static string ImageTrackingHelperScene = "ImageTrackingHelper";
    public static string CloudImageTrackingScene = "CloudImageTracking";
    public static string Video360Scene = "Video360Scene";
    public static string SplashScene = "SplashScene";
    public static string EmailEntryScene = "EmailEntryScene";
    public static string ARLensScene = "ARLensScene";

}
