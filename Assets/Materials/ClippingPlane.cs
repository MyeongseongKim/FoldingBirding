using System.Collections.Generic;
using UnityEngine;


// [ExecuteAlways]
public class ClippingPlane : MonoBehaviour 
{
    // public Material mat;

    // void Update () 
    // {
    //     Plane plane = new Plane(transform.forward, transform.position);
    //     Vector4 planeRepresentation = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
    //     mat.SetVector("_Plane", planeRepresentation);
    // }

    [SerializeField] private Shader _shader;
    [SerializeField] private GameObject _envBefore;
    [SerializeField] private GameObject _envAfter;

    private List<Renderer> _beforeRenderers = new List<Renderer>();
    private List<Renderer> _afterRenderers  = new List<Renderer>();


    void Start()
    {
        if (_shader == null)
        {
            Debug.LogError("[ClippingPlane] Shader is not assigned!");
            return;
        }

        _beforeRenderers = new List<Renderer>(
            _envBefore.GetComponentsInChildren<Renderer>(true)
        );
        _afterRenderers = new List<Renderer>(
            _envAfter.GetComponentsInChildren<Renderer>(true)
        );

        ApplyShader(_beforeRenderers);
        ApplyShader(_afterRenderers);
    }


    void Update() 
    {
        var plane = new Plane(transform.forward, transform.position);
        Vector4 planeVec = new Vector4(
            plane.normal.x,
            plane.normal.y,
            plane.normal.z,
            plane.distance
        );
        Vector4 invPlaneVec = -planeVec;

        foreach (var rend in _beforeRenderers)
            rend.material.SetVector("_Plane", planeVec);

        foreach (var rend in _afterRenderers)
            rend.material.SetVector("_Plane", invPlaneVec);
    }


    void ApplyShader(List<Renderer> renderers)
    {
        foreach (var rend in renderers)
        {
            var mats = rend.materials;  // material → 인스턴스화된 copy
            for (int i = 0; i < mats.Length; i++)
                mats[i].shader = _shader;
            rend.materials = mats;
        }
    }


    // void ApplyShader(GameObject root)
    // {
    //     if (root == null || _shader == null)
    //     {
    //         Debug.LogWarning("ClippingPlane: Root or Shader is null!");
    //         return;
    //     }

    //     var renderers = root.GetComponentsInChildren<Renderer>(includeInactive: true);
    //     foreach (var rend in renderers)
    //     {
    //         Material[] mats = rend.materials;    
    //         for (int i = 0; i < mats.Length; i++)
    //         {
    //             mats[i].shader = _shader;
    //         }
    //         rend.materials = mats;
    //     }
    // }
}