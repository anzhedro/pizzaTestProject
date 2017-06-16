using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CutPizza : MonoBehaviour
{
    static public bool buttonPress = false;
    public Transform point1;
    public Transform point2;

    public GameObject pizza;
    public GameObject pizzaSlice;

    public Text pizzaAreaText;
    public Text pizzaSliceAreaText;
    public Text cutsCountText;

    float fullPizzaArea = 0.0f, cutsCount = 0.0f;
    public void doCut()
    {
        Mesh mesh = pizza.GetComponent<MeshFilter>().mesh;

        List<Vector3> newVertexList = new List<Vector3>(); // part 1
        List<int> newTriangleList = new List<int>();
        Mesh newMesh = new Mesh();
        newMesh.name = "PizzaPart1";

        List<Vector3> newSliceVertexList = new List<Vector3>(); //part 2
        List<int> newSliceTriangleList = new List<int>();
        Mesh newSliceMesh = new Mesh();
        newMesh.name = "PizzaPart2";

        Vector3 p1 = point1.position, p2 = point2.position, p3, p4; // points

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
        if (num == 2)
        //if (num >= 1)
        {
            anyCrossed = true;
        }


        if (anyCrossed)
        {
            Vector3 newVertex = new Vector3();
            int crossSidesTriangle = 0, k = 0, firstValueIndex = 0;

            bool isRight = false;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                p3 = mesh.vertices[mesh.triangles[i + 1]];
                p4 = mesh.vertices[mesh.triangles[i]];

                d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
                da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
                db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

                ta = da / d;
                tb = db / d;

                if (k == 1)
                {
                    newVertexList.Add(mesh.vertices[i / 3 + 1]);
                    isRight = true;
                }

                if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1) // if crossed with side
                {
                    dx = p1.x + ta * (p2.x - p1.x);
                    dy = p1.y + ta * (p2.y - p1.y);
                    newVertex = new Vector3(dx, dy, 0.0f);
                    crossSidesTriangle = i / 3;

                    if (k == 0)
                    {
                        newVertexList.Add(newVertex);

                        newSliceVertexList.Add(mesh.vertices[crossSidesTriangle + 1]);
                        newSliceVertexList.Add(newVertex);
                    }
                    else
                    {
                        newVertexList.Add(newVertex);

                        firstValueIndex = newSliceVertexList.Count;  // offset
                        newSliceVertexList.Add(newVertex);
                        newSliceVertexList.Add(mesh.vertices[crossSidesTriangle + 2]);
                    }
                    isRight = true;
                    k += 1;
                }

                if (!isRight)
                {
                    newSliceVertexList.Add(mesh.vertices[i / 3 + 1]);
                }
                isRight = false;

            }
            // Debug.Log("K: " + k);


            newSliceVertexList = newSliceVertexList.Skip(firstValueIndex).Concat(newSliceVertexList.Take(firstValueIndex)).ToList(); // offset firstValueIndex


            // 0 point
            Vector3 middle = new Vector3((newVertexList[0].x + newVertexList[newVertexList.Count - 1].x) / 2, (newVertexList[0].y + newVertexList[newVertexList.Count - 1].y) / 2, 0.0f);

            newVertexList.Insert(0, middle);
            newSliceVertexList.Insert(0, middle);

            // 1 part
            newVertexList.Add(newVertexList[1]);
            newMesh.vertices = newVertexList.ToArray();

            newTriangleList = calcTriangles(newVertexList.Count);

            newMesh.triangles = newTriangleList.ToArray();

            newMesh.uv = calcUVS(newMesh.vertices);
            newMesh.RecalculateNormals();

            // 2 part
            newSliceVertexList.Add(newSliceVertexList[1]);
            newSliceMesh.vertices = newSliceVertexList.ToArray();

            newSliceTriangleList = calcTriangles(newSliceVertexList.Count);
            newSliceMesh.triangles = newSliceTriangleList.ToArray();

            newSliceMesh.uv = calcUVS(newSliceMesh.vertices);
            newSliceMesh.RecalculateNormals();


            float firstPartArea = calcPolygonArea(newMesh.vertices);
            float secondPartArea = calcPolygonArea(newSliceMesh.vertices);


            if (cutsCount == 0)
            {
                fullPizzaArea = firstPartArea + secondPartArea;
                Debug.Log("Total pizza:" + fullPizzaArea);
            }



            if (firstPartArea > secondPartArea)
            {
                buildSlice(newSliceMesh);
                updatePizzaMesh(newMesh);
                Debug.Log("Pizza area:" + firstPartArea);
                Debug.Log("Slice area:" + secondPartArea);
                updateArea(firstPartArea, secondPartArea);
            }
            else
            {
                buildSlice(newMesh);
                updatePizzaMesh(newSliceMesh);
                Debug.Log("Pizza area:" + secondPartArea);
                Debug.Log("Slice area:" + firstPartArea);
                updateArea(secondPartArea, firstPartArea);
            }

            buttonPress = true; // for AreaHandle
        }
        else
        {
            Debug.Log("Not crossed");
        }
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

    List<int> calcTriangles(int n)
    {
        List<int> newTriangleList = new List<int>();
        for (int i = 2; i < n; i++)
        {
            newTriangleList.Add(i);
            newTriangleList.Add(i - 1);
            newTriangleList.Add(0);
        }

        return newTriangleList;
    }

    Vector2[] calcUVS(Vector3[] vert)
    {
        Vector2[] uvs = new Vector2[vert.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.5f + (vert[i].x - 0.0f) / (2f * 1f), 0.5f + (vert[i].y - 0.0f) / (2f * 1f));
        }
        return uvs;
    }

    void buildSlice(Mesh sliceMesh)
    {
        GameObject pSlice = Instantiate(pizzaSlice, pizza.transform.position, Quaternion.identity);
        Destroy(pSlice.GetComponent<MeshCollider>());
        pSlice.AddComponent<MeshFilter>();
        pSlice.GetComponent<MeshFilter>().mesh = sliceMesh;
        pSlice.AddComponent<Rigidbody2D>();
    }

    void updatePizzaMesh(Mesh newMesh)
    {
        pizza.GetComponent<MeshFilter>().mesh = newMesh;
        pizza.GetComponent<MeshCollider>().sharedMesh = newMesh;
    }

    void updateArea(float pArea, float sArea)
    {
        float pizzaArea = ((pArea / fullPizzaArea) * 100);
        float pizzaSliceArea = ((sArea / fullPizzaArea) * 100);
        if (sArea > 0.00001f) {
            cutsCount++;
            pizzaAreaText.text = "Pizza area: " + pizzaArea + "%";
            pizzaSliceAreaText.text = "Slice area: " + pizzaSliceArea + "%";
            cutsCountText.text = "Cuts count: " + cutsCount;
        }
    }

    float calcPolygonArea(Vector3[] points)
    {
        float area = 0.0f;
        for (int i = 1; i < points.Length - 1; i++)
        {
            area += (points[i].x + points[i + 1].x) * (points[i].y - points[i + 1].y);
        }
        area = Mathf.Abs(area) / 2;
        return area;
    }
}
