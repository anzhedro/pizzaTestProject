using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutPizza : MonoBehaviour
{
    static public bool buttonPress = false;
    public Transform point1;
    public Transform point2;
    public GameObject pizza;
    public GameObject pizzaSlice;

    Vector3[] tempVertex;
    void Awake()
    {
    }
    public void doSomething()
    {
        Mesh mesh = pizza.GetComponent<MeshFilter>().mesh;
        Mesh newMesh = new Mesh();
        newMesh.name = "PizzaNew";
        
        List<Vector3> newVertexList = new List<Vector3>();
        List<int> newTriangleList = new List<int>();

        tempVertex = new Vector3[mesh.vertexCount + 2]; 

        Vector3 p1 = point1.position, p2 = point2.position, p3, p4; // points

        Vector3[] mid = new Vector3[2]; // first point of slice

        float d = 0.0f, da = 0, db = 0, ta = 0, tb = 0, dx = 0, dy = 0;
         
        bool anyCrossed = false; // line crossed pizza
        int num = 0;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            p3 = mesh.vertices[mesh.triangles[i + 1]];
            p4 = mesh.vertices[mesh.triangles[i]];
            if (isCrossing(p1, p2, p3, p4))
            {
                num++;
            }
        }
        if (num >= 2)
        {
            anyCrossed = true;
        }


        // ----------------------------------------------- Add new vertices
        if (anyCrossed)
        {
            newVertexList = mesh.vertices.ToList();
            Vector3 newVertex = new Vector3(), nVert = new Vector3();
            int crossSidesTriangle = 0, crST = 0, k = 0;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                p3 = mesh.vertices[mesh.triangles[i + 1]];
                p4 = mesh.vertices[mesh.triangles[i]];

                d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
                da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
                db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

                ta = da / d;
                tb = db / d;

                if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
                {
                    dx = p1.x + ta * (p2.x - p1.x);
                    dy = p1.y + ta * (p2.y - p1.y);
                    newVertex = new Vector3(dx, dy, 0.0f);
                    crossSidesTriangle = i / 3;
                    if (k == 0)
                    {
                        nVert = newVertex;
                        crST = crossSidesTriangle;
                    }
                    newVertexList.Insert(crossSidesTriangle + 2 + k, newVertex);
                    mid[k % 2] = newVertex;
                    k += 1;
                }
            }

            for (int i = 2; i < newVertexList.Count; i++)
            {
                newTriangleList.Add(i);
                newTriangleList.Add(i - 1);
                newTriangleList.Add(0);
            }

            newMesh.vertices = newVertexList.ToArray();
            newMesh.triangles = newTriangleList.ToArray();

            tempVertex = new Vector3[newMesh.vertexCount + 2]; // slice vertices
            tempVertex[0] = new Vector3((mid[0].x + mid[1].x) / 2, (mid[0].y + mid[1].y) / 2, 0.0f);
            //Debug.Log("Tr: " + crST + " " + nVert);
            tempVertex[crST + 2] = nVert;
            //Debug.Log("Tr: " + crossSidesTriangle + " " + newVertex);
            //tempVertex[crossSidesTriangle + 2 + (k-1)] = newVertex;


            // ---------------------------------------------------------- Cut pizza
            bool isCrossedWith0 = false;
            for (int i = 0; i < newMesh.triangles.Length; i += 3)
            {
                p3 = newMesh.vertices[newMesh.triangles[i]];
                p4 = newMesh.vertices[newMesh.triangles[i + 2]];


                d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
                da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
                db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

                ta = da / d;
                tb = db / d;

                if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
                {
                    dx = p1.x + ta * (p2.x - p1.x);
                    dy = p1.y + ta * (p2.y - p1.y);
                    newVertex = new Vector3(dx, dy, 0.0f);
                    crossSidesTriangle = i / 3;

                    if (i == 0)
                    {
                        isCrossedWith0 = true;
                        continue;
                    }

                    Vector3[] vertices = newMesh.vertices;

                    tempVertex[crossSidesTriangle + 1] = vertices[crossSidesTriangle + 1];
                    vertices[(crossSidesTriangle + 1)] = newVertex;
                    newMesh.vertices = vertices;
                }
            }

            if (isCrossedWith0 == true)
            {
                Vector3[] vertices = newMesh.vertices;
                p3 = newMesh.vertices[0];
                p4 = newMesh.vertices[1];

                d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
                da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
                dx = p1.x + da / d * (p2.x - p1.x);
                dy = p1.y + da / d * (p2.y - p1.y);

                newVertex = new Vector3(dx, dy, 0.0f);
                tempVertex[1] = vertices[1];
                vertices[1] = newVertex;
                newMesh.vertices = vertices;
            }

            Vector2[] uvs = new Vector2[newMesh.vertices.Length];
            for (int m = 0; m < uvs.Length; m++)
            {
                uvs[m] = new Vector2(0.5f + (newMesh.vertices[m].x - 0.0f) / (2f * 1f), 0.5f + (newMesh.vertices[m].y - 0.0f) / (2f * 1f));
            }
            newMesh.uv = uvs;
            newMesh.RecalculateNormals();

            pizza.GetComponent<MeshFilter>().mesh = newMesh;
            pizza.GetComponent<MeshCollider>().sharedMesh = newMesh;



            buildSilce();   // Build slice;

            buttonPress = true; // for AreaHandle
        }
        else
        {
            Debug.Log("Not crossed");
        }
    }

    void buildSilce()
    {
        Mesh newSliceMesh = new Mesh();
        newSliceMesh.name = "Pizza Slice";
        List<Vector3> newSliceVertexList = new List<Vector3>();
        List<int> newSliceTriangleList = new List<int>();

        for (int i = 0; i < tempVertex.Length; i++)
        {
            if (tempVertex[i] != new Vector3(0, 0, 0))
            {
                newSliceVertexList.Add(tempVertex[i]);
            }
        }
        newSliceVertexList.Add(newSliceVertexList[1]);

        for (int i = 2; i < newSliceVertexList.Count; i++)
        {
            newSliceTriangleList.Add(i);
            newSliceTriangleList.Add(i - 1);
            newSliceTriangleList.Add(0);
        }

        newSliceMesh.vertices = newSliceVertexList.ToArray();
        newSliceMesh.triangles = newSliceTriangleList.ToArray();

        Vector2[] uvs = new Vector2[newSliceMesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.5f + (newSliceVertexList[i].x - 0.0f) / (2f * 1f),
                             0.5f + (newSliceVertexList[i].y - 0.0f) / (2f * 1f));
        }
        newSliceMesh.uv = uvs;

        newSliceMesh.RecalculateNormals();

        GameObject pSlice = Instantiate(pizzaSlice, pizza.transform.position, Quaternion.identity);
        Destroy(pSlice.GetComponent<MeshCollider>());
        pSlice.AddComponent<MeshFilter>();
        pSlice.GetComponent<MeshFilter>().mesh = newSliceMesh;
        pSlice.AddComponent<Rigidbody2D>();
    }

    bool isCrossing(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
        float da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
        float db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

        float ta = da / d;
        float tb = db / d;

        if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
        {
            return true;
        }
        return false;
    }
}
