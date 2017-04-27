using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFromPoin1toPoint2 : MonoBehaviour {
    private LineRenderer _lineRenderer;
    // Use this for initialization
    void Start () {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 1f;
        _lineRenderer.endWidth = 1f;

        UpdateLine();
    }
	
	// Update is called once per frame
	void Update () {
        if (DragObject.dragging || DragObject.touchDragging)
        {
            UpdateLine();
        }
    }

    void UpdateLine()
    {
        _lineRenderer.SetPosition(0, gameObject.transform.GetChild(0).position);
        _lineRenderer.SetPosition(1, gameObject.transform.GetChild(1).position);
    }
}
