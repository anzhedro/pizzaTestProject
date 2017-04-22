using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public Material mat;
    public void Start()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 1f;
        _lineRenderer.endWidth = 1f;
        _lineRenderer.enabled = false;
        _lineRenderer.material = mat;
    }

    private Vector3 _initialPosition;
    private Vector3 _currentPosition;
    bool drawStarted = false;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Wall")
                {
                    drawStarted = true;
                    _initialPosition = GetCurrentMousePosition().GetValueOrDefault();
                    Debug.Log("Line start: " + _initialPosition);
                    _lineRenderer.SetPosition(0, _initialPosition);
                    _lineRenderer.positionCount = 1;
                    _lineRenderer.enabled = true;
                }
            }
        }
        else if (Input.GetMouseButton(0) && drawStarted)
        {
            _currentPosition = GetCurrentMousePosition().GetValueOrDefault();
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(1, _currentPosition);

        }
        else if (Input.GetMouseButtonUp(0) && drawStarted)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Wall")
                {
                    _lineRenderer.enabled = false;
                    var releasePosition = GetCurrentMousePosition().GetValueOrDefault();
                    Debug.Log("Line end: " + releasePosition);
                    var direction = releasePosition - _initialPosition;
                    Debug.Log("Process direction " + direction);
                }
                else
                {
                    _lineRenderer.enabled = false;
                }
            }
            drawStarted = false;
        }
    }

    private Vector3? GetCurrentMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, Vector3.zero);

        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);

        }

        return null;
    }

}