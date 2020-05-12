using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour
{

    MeshRenderer myMesh;

    // Start is called before the first frame update
    void Start()
    {
        myMesh = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGamePaused)
        {
            myMesh.enabled = false;
        } else
        {
            myMesh.enabled = true;
        }
    }
}
