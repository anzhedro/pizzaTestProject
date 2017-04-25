using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BuildPizza : MonoBehaviour
{
    public GameObject pizzaSlice;
    public int numOfPoints = 90;

    private Mesh mesh;

    float centerX = 0.0f;
    float centerY = 0.0f;
    float centerZ = 0.0f;
    float radius = 1.0f;
    float angleStep = 0.0f;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Pizza";

        angleStep = 360.0f / (float)numOfPoints;
    }
    void Start()
    {
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
        
        // Make first triangle.
        vertexList.Add(new Vector3(centerX, centerY, centerZ));  // 1. Circle center.
        vertexList.Add(new Vector3(0.0f, radius, 0.0f));         // 2. First vertex on circle outline
        vertexList.Add(quaternion * vertexList[1]);             // 3. First vertex on circle outline rotated by angle)

        triangleList.Add(2); // Add triangle indices.
        triangleList.Add(1);
        triangleList.Add(0);
        for (int i = 0; i < numOfPoints - 1; i++)
        {
            triangleList.Add(vertexList.Count);                      
            triangleList.Add(vertexList.Count - 1);
            triangleList.Add(0);                            // Index of circle center.

            vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
        }
        vertexList[vertexList.Count - 1] = vertexList[1];
        triangleList[(triangleList.Count - 3)] = triangleList[1];

        mesh.vertices = vertexList.ToArray();

        mesh.triangles = triangleList.ToArray();


        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.5f + (vertexList[i].x - centerX) / (2 * radius), 0.5f + (vertexList[i].y - centerY) / (2 * radius));
        }
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        MeshCollider col = GetComponent<MeshCollider>();
        col.sharedMesh = mesh;

    }
    //void deleteTri(int index, GameObject obj)
    //{
    //    Destroy(obj.GetComponent<MeshCollider>());
    //    Mesh newMesh = obj.GetComponent<MeshFilter>().mesh;
    //    int[] oldTriangles = newMesh.triangles;
    //    int[] newTriangles = new int[newMesh.triangles.Length - 3];
    //    int i = 0, j = 0;

    //    int[] pSliceTr = new int[3];
    //    while (j < newMesh.triangles.Length)
    //    {
    //        if (j != index * 3)
    //        {
    //            newTriangles[i++] = oldTriangles[j++];
    //            newTriangles[i++] = oldTriangles[j++];
    //            newTriangles[i++] = oldTriangles[j++];
    //        }
    //        else
    //        {
    //            pSliceTr[0] = oldTriangles[j++];
    //            pSliceTr[1] = oldTriangles[j++];
    //            pSliceTr[2] = oldTriangles[j++];
    //        }
    //    }
    //    GameObject pSlice = Instantiate(pizzaSlice, transform.position, Quaternion.identity);
    //    Destroy(pSlice.GetComponent<MeshCollider>());

    //    pSlice.AddComponent<MeshFilter>();
    //    pSlice.GetComponent<MeshFilter>().mesh = obj.GetComponent<MeshFilter>().mesh;
    //    pSlice.GetComponent<MeshFilter>().mesh.triangles = pSliceTr;
    //    pSlice.AddComponent<Rigidbody2D>();

    //    //pizzaArea -= pizzaSliceArea;
    //   // pizzaAreaText.text = "Pizza area: " + Mathf.Round(pizzaArea / fullPizzaArea * 100).ToString() + "%";

    //    obj.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
    //    obj.AddComponent<MeshCollider>();

    //}
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Pizza")
                {
                    Debug.Log(hit.triangleIndex);
                    //deleteTri(hit.triangleIndex, hit.transform.gameObject);
                }
            }
        }

        // Touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Touch myTouch = Input.GetTouch(0);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Pizza")
                {
                    Debug.Log(hit.triangleIndex);
                    //deleteTri(hit.triangleIndex, hit.transform.gameObject);
                }
            }
        }
    }
}