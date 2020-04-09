using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour
{
    //public enum RenderPlane
    //{
    //    NONE,
    //    FLOOR_CEIL,
    //    PERPENDICULAR_WALL, //respecto la camara
    //    PARALLEL_WALL, //respecto la camara
    //}

    //public RenderPlane renderPlane;

    private float tileX;
    private float tileY;
    private float tileZ;
    private float matX;
    private float matY;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        tileX = GetComponent<Transform>().localScale.x;
        tileY = GetComponent<Transform>().localScale.y;
        tileZ = GetComponent<Transform>().localScale.z;
        mat.mainTextureScale = new Vector2(tileX, tileZ);
    }

    void Update()
    {
        //if (renderPlane == RenderPlane.PERPENDICULAR_WALL)
        //{
        //    mat.mainTextureScale = new Vector2(tileZ, tileY);
        //}
        //else if (renderPlane == RenderPlane.PARALLEL_WALL)
        //{
        //    mat.mainTextureScale = new Vector2(tileX, tileY);
        //}
        //else if (renderPlane == RenderPlane.FLOOR_CEIL)
        //{
        //    mat.mainTextureScale = new Vector2(tileX, tileZ);
        //}    
    }
}