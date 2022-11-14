using UnityEngine;
using System.Collections;

public class InvertObjectNormals : MonoBehaviour 
{
    public GameObject SferaPanoramica;

    void Awake()
    {

        if (SferaPanoramica == null)
            SferaPanoramica = gameObject;
        
        InvertSphere();
    }

    void InvertSphere()
    {
        Vector3[] normals = SferaPanoramica.GetComponent<MeshFilter>().mesh.normals;
        
        for(int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        
        SferaPanoramica.GetComponent<MeshFilter>().mesh.normals = normals;

        int[] triangles = SferaPanoramica.GetComponent<MeshFilter>().mesh.triangles;
        
        for (int i = 0; i < triangles.Length; i+=3)
        {
            int t = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = t;
        }           

        SferaPanoramica.GetComponent<MeshFilter>().mesh.triangles= triangles;
    }
}

