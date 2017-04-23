using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAngle : MonoBehaviour
{
    public GameObject center;
    public GameObject line1;
    public GameObject line2;
    LineRenderer line;
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 3;
        line.startWidth = 1f;
        line.endWidth = 1f;
        CreatePoints();
    }


    void CreatePoints()
    {
        line.SetPosition(0, line1.transform.position);
        line.SetPosition(1, center.transform.position);
        line.SetPosition(2, line2.transform.position);
    }
    void Update()
    {
        CreatePoints();
    }
}
