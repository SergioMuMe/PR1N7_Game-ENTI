using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer lineR;
    private Vector3[] pos;

    public Transform[] points;

    void Start()
    {
        lineR = GetComponent<LineRenderer>();

        pos = new Vector3[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            pos[i] = points[i].position;
        }

        lineR.SetPositions(pos);
    }
}
