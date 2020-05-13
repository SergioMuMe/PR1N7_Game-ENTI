using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    //workaround para parchear: https://issuetracker.unity3d.com/issues/ugui-canvas-with-world-space-render-mode-is-always-rendered-before-a-transparent-mesh-renderer
    MeshRenderer myMesh;
    HUDController canvasHUD;

    // Start is called before the first frame update
    void Start()
    {
        myMesh = gameObject.GetComponent<MeshRenderer>();
        canvasHUD = GameObject.Find("CanvasHUD").GetComponent<HUDController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGamePaused && !myMesh.enabled)
        {
            myMesh.enabled = true;
        }

        if (GameManager.Instance.isGamePaused || canvasHUD.levelEnded)
        {   
            myMesh.enabled = false;
        } else
        {
            myMesh.enabled = true;
        }
    }
}
