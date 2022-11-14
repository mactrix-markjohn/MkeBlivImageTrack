Shader "Unlit/Mask"
{
    Properties
    {
       
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Zwrite Off
        }
    }
}
