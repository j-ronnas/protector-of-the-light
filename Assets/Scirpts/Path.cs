using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    List<Vector2> points;
    MapManager mapManager;

    float animationSpeed = 0.5f;

    public Vector2 GetPosition(int index)
    {
        if( index < points.Count)
        {
            return points[index];
        }
        return -Vector2.one;
    }

    public void Init(Vector2 start, Vector2 end)
    {

        mapManager = FindObjectOfType<MapManager>();
        points = mapManager.findPath(start, end);
        Vector3[] vArr = new Vector3[points.Count];

        for (int i = 0; i < vArr.Length; i++)
        {
            vArr[i] = new Vector3(points[i].x, points[i].y, 0.5f) - new Vector3(0, 0.3f, 0) ;
        }
        GetComponent<LineRenderer>().positionCount = vArr.Length;
        GetComponent<LineRenderer>().SetPositions(vArr);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<LineRenderer>().material.mainTextureOffset -= new Vector2(Time.deltaTime* animationSpeed, 0);
    }

    public bool ContainsPos(Vector2 pos)
    {
        return points.Contains(pos);
    }
}
