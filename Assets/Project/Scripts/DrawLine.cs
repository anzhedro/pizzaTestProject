using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public Material lineMaterial;
    public Material sqMaterial;
    public void Start()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 1f;
        _lineRenderer.endWidth = 1f;
        _lineRenderer.enabled = false;
        _lineRenderer.material = lineMaterial;
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
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.tag);
                if (hit.transform.tag != "Pizza")
                {
                    drawStarted = true;
                    _initialPosition = GetCurrentMousePosition().GetValueOrDefault();
                    _initialPosition.z = -2.0f;
                    _lineRenderer.SetPosition(0, _initialPosition);
                    _lineRenderer.positionCount = 1;
                    _lineRenderer.enabled = true;
                }
            }
        }
        else if (Input.GetMouseButton(0) && drawStarted)
        {
            _currentPosition = GetCurrentMousePosition().GetValueOrDefault();
            _currentPosition.z = -2.0f;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(1, _currentPosition);

        }
        else if (Input.GetMouseButtonUp(0) && drawStarted)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag != "Pizza")
                {
                    _lineRenderer.enabled = false;
                    var _releasePosition = GetCurrentMousePosition().GetValueOrDefault();

                    //Debug.Log("Process direction " + direction);

                    _releasePosition.z = 0.0f;
                    _initialPosition.z = 0.0f;
                    var direction = _releasePosition - _initialPosition;
                    OnDrawEnd(_initialPosition, _releasePosition, direction);
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

    Mesh CreateMesh(float len)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
         new Vector3(-len, -len, 0.01f),
         new Vector3(len, -len, 0.01f),
         new Vector3(len, len, 0.01f),
         new Vector3(-len, len, 0.01f)
     };
        m.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2(1, 1),
         new Vector2 (1, 0)
     };
        m.triangles = new int[] { 2, 1, 0, 3, 2, 0 };
        m.RecalculateNormals();

        return m;
    }

    int planeCount = 0;
    void OnDrawEnd(Vector3 firstPoint, Vector3 secondPoint, Vector3 dir)
    {
        float size = Vector3.Distance(firstPoint, secondPoint);


        GameObject plane = new GameObject("Cut Plane " + planeCount++.ToString());
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(size / 2);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = sqMaterial;

        //renderer.material.shader = Shader.Find("Particles/Additive");
        //Texture2D tex = new Texture2D(1, 1);
        //tex.SetPixel(0, 0, Color.gray);
        //tex.Apply();
        //renderer.material.mainTexture = tex;
        //renderer.material.color = Color.gray;
        plane.AddComponent<MeshCollider>();
        plane.transform.tag = "Wall";
        plane.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        Vector3 a = firstPoint;
        Vector3 b = secondPoint;
        Vector3 c = new Vector3(firstPoint.x + (-(secondPoint.y - firstPoint.y)), firstPoint.y + (secondPoint.x - firstPoint.x));
        Vector3 d = new Vector3(secondPoint.x + (-(secondPoint.y - firstPoint.y)), secondPoint.y + (secondPoint.x - firstPoint.x));
        Debug.DrawLine(a, c, Color.red, 5.0f);
        Debug.DrawLine(c, d, Color.red, 5.0f);
        Debug.DrawLine(d, b, Color.red, 5.0f);
        Debug.DrawLine(b, a, Color.red, 5.0f);

        Vector3 mid = new Vector3((a.x + d.x) / 2, (c.y + b.y) / 2, -0.1f);

        Debug.DrawLine(mid, a, Color.blue, 5.0f);


        plane.transform.position = mid;
    }
}