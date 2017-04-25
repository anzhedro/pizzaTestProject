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

        tempVertex = new Vector3[mesh.vertexCount];

        Vector3 p1 = point1.position;
        Vector3 p2 = point2.position;
        Vector3[] mid = new Vector3[2];

        float d = 0.0f, da = 0, db = 0, ta = 0, tb = 0 , dx = 0 ,dy = 0;
        // Debug.DrawRay(p1, (p2 - p1), Color.red, 5.0f);

        bool flag = false;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p3 = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 p4 = mesh.vertices[mesh.triangles[i]];
            // Debug.DrawRay(p4, (p3 - p4), Color.blue, 5.0f);


             d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
             da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
             db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

             ta = da / d;
             tb = db / d;

            if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
            {
                 dx = p1.x + ta * (p2.x - p1.x);
                 dy = p1.y + ta * (p2.y - p1.y);
                Vector3 newVertex = new Vector3(dx, dy, 0.0f);
                int crossSidesTriangle = i / 3;

                Vector3[] vertices = mesh.vertices;

                if (!flag)
                {
                    mid[0] = newVertex;
                    //Debug.Log("a " + (crossSidesTriangle + 1));
                    //tempVertex[crossSidesTriangle + 1] = vertices[crossSidesTriangle + 1];
                    vertices[crossSidesTriangle + 1] = newVertex;
                    tempVertex[crossSidesTriangle + 1] = vertices[crossSidesTriangle + 1];
                    mesh.vertices = vertices;
                    flag = true;
                }
                else if (flag)
                {
                    mid[1] = newVertex;
                    //Debug.Log("b " + (crossSidesTriangle + 2));
                    tempVertex[crossSidesTriangle + 2] = vertices[crossSidesTriangle + 2];
                    vertices[crossSidesTriangle + 2] = newVertex;
                    //tempVertex[crossSidesTriangle + 2] = vertices[crossSidesTriangle + 2];
                    mesh.vertices = vertices;
                    flag = false;
                }

                Vector2[] uvs1 = new Vector2[mesh.vertices.Length];
                for (int m = 0; m < uvs1.Length; m++)
                {
                    uvs1[m] = new Vector2(0.5f + (mesh.vertices[m].x - 0.0f) / (2f * 1f), 0.5f + (mesh.vertices[m].y - 0.0f) / (2f * 1f));
                }
                mesh.uv = uvs1;

                mesh.RecalculateNormals();

                pizza.GetComponent<MeshFilter>().mesh = mesh;
                pizza.GetComponent<MeshCollider>().sharedMesh = mesh;
            }
        }


        tempVertex[0] = new Vector3((mid[0].x + mid[1].x) / 2, (mid[0].y + mid[1].y) / 2, 0.0f);
        // Debug.DrawLine(tempVertex[0], new Vector3(0, 0, 0), Color.blue, 5.0f);

        bool flag0 = false;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p3 = mesh.vertices[mesh.triangles[i]];
            Vector3 p4 = mesh.vertices[mesh.triangles[i + 2]];


             d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
             da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
             db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

             ta = da / d;
             tb = db / d;

            if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
            {
                 dx = p1.x + ta * (p2.x - p1.x);
                 dy = p1.y + ta * (p2.y - p1.y);
                Vector3 newVertex = new Vector3(dx, dy, 0.0f);
                int crossSidesTriangle = i / 3;

                if (i == 0)
                {
                    flag0 = true;
                    continue;
                }

                Vector3[] vertices = mesh.vertices;

                Vector2[] uvs1 = new Vector2[mesh.vertices.Length];
                for (int m = 0; m < uvs1.Length; m++)
                {
                    uvs1[m] = new Vector2(0.5f + (mesh.vertices[m].x - 0.0f) / (2f * 1f), 0.5f + (mesh.vertices[m].y - 0.0f) / (2f * 1f));
                }
                mesh.uv = uvs1;

                mesh.RecalculateNormals();

                tempVertex[crossSidesTriangle + 1] = vertices[crossSidesTriangle + 1];
                vertices[(crossSidesTriangle + 1)] = newVertex;
                mesh.vertices = vertices;
                pizza.GetComponent<MeshFilter>().mesh = mesh;
                pizza.GetComponent<MeshCollider>().sharedMesh = mesh;
            }
        }

        if (flag0 == true)
        {
            Vector3[] vertices = mesh.vertices;
            Vector3 p3 = mesh.vertices[0];
            Vector3 p4 = mesh.vertices[1];

             d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
             da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);

             ta = da / d;

             dx = p1.x + ta * (p2.x - p1.x);
             dy = p1.y + ta * (p2.y - p1.y);

            Vector3 newVertex = new Vector3(dx, dy, 0.0f);
            tempVertex[1] = vertices[1];
            vertices[1] = newVertex;
            mesh.vertices = vertices;

            Vector2[] uvs1 = new Vector2[mesh.vertices.Length];
            for (int m = 0; m < uvs1.Length; m++)
            {
                uvs1[m] = new Vector2(0.5f + (mesh.vertices[m].x - 0.0f) / (2f * 1f),
                                 0.5f + (mesh.vertices[m].y - 0.0f) / (2f * 1f));
                //Debug.Log(i.ToString() + " " + uvs[i].ToString());
            }
            mesh.uv = uvs1;

            mesh.RecalculateNormals();

            pizza.GetComponent<MeshFilter>().mesh = mesh;
            pizza.GetComponent<MeshCollider>().sharedMesh = mesh;

        }



        buildSilce();

        buttonPress = true;
    }

    void buildSilce()
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "Pizza Slice";
        List<Vector3> newVertexList = new List<Vector3>();
        List<int> newTriangleList = new List<int>();

        for (int i = 0; i < tempVertex.Length; i++)
        {
            if (tempVertex[i] != new Vector3(0, 0, 0))
            {
                // Debug.DrawLine(tempVertex[i], tempVertex[0], Color.yellow, 5.0f);
                newVertexList.Add(tempVertex[i]);
            }
        }
        newVertexList.Add(newVertexList[1]);
        //Debug.Log(newVertexList.Count);

        for (int i = 2; i < newVertexList.Count; i++)
        {
            newTriangleList.Add(i);
            newTriangleList.Add(i - 1);
            newTriangleList.Add(0);
        }
        //Debug.Log(newTriangleList.Count);

        newMesh.vertices = newVertexList.ToArray();
        newMesh.triangles = newTriangleList.ToArray();

        Vector2[] uvs = new Vector2[newMesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.5f + (newVertexList[i].x - 0.0f) / (2f * 1f),
                             0.5f + (newVertexList[i].y - 0.0f) / (2f * 1f));
        }
        newMesh.uv = uvs;

        newMesh.RecalculateNormals();

        GameObject pSlice = Instantiate(pizzaSlice, pizza.transform.position, Quaternion.identity);
        Destroy(pSlice.GetComponent<MeshCollider>());
        pSlice.AddComponent<MeshFilter>();
        pSlice.GetComponent<MeshFilter>().mesh = newMesh;
        pSlice.AddComponent<Rigidbody2D>();
    }
    //bool isCrossing(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    //{
    //    float d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
    //    float da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
    //    float db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

    //    float ta = da / d;
    //    float tb = db / d;

    //    if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
    //    {
    //        float dx = p1.x + ta * (p2.x - p1.x);
    //        float dy = p1.y + ta * (p2.y - p1.y);
    //        Debug.Log("Point " + dx + " " + dy);
    //        return true;
    //    }
    //    return false;
    //}
}
