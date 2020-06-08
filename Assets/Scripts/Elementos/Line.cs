using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer lineR;
    private List<Vector3> pos;

    public Transform[] points;

    void Start()
    {
        lineR = GetComponent<LineRenderer>();

        LinkTheLine();
    }

    public void LinkTheLine()
    {
        pos = new List<Vector3>();

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].gameObject.activeSelf)
            {
                pos.Add(points[i].localPosition);
            }
        }

        lineR.positionCount = pos.Count;

        lineR.SetPositions(pos.ToArray());
    }
}
