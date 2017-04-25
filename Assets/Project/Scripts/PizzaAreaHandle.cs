using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PizzaAreaHandle : MonoBehaviour {

    float fullPizzaArea = 0.0f;
    //float radius = 1.0f;
    float pizzaArea = 0.0f;
    //static float pizzaSliceArea;
    public GameObject Pizza;
    public Text pizzaAreaText;
    public Text pizzaSliceAreaText;
    Mesh mesh;
    void Awake()
    {
        //fullPizzaArea = radius * radius * Mathf.PI;
        //pizzaArea = fullPizzaArea;
        //pizzaSliceArea = (angleStep / 360.0f) * radius * radius * Mathf.PI;
    }
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (CutPizza.buttonPress) {
            mesh = Pizza.GetComponent<MeshFilter>().mesh;
            Debug.Log(mesh.triangles.Length);
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                float a = calcTriangleArea(vertices[mesh.triangles[i]], vertices[mesh.triangles[i + 1]], vertices[mesh.triangles[i + 2]]);
                if (!System.Single.IsNaN(a))
                { 
                    pizzaArea += a;
                }
               // Debug.Log(vertices[mesh.triangles[i]] + " " + vertices[mesh.triangles[i+1]] + " " + vertices[mesh.triangles[i+2]]);
            }
            Debug.Log(pizzaArea);
            CutPizza.buttonPress = false;
        }
    }

    float calcTriangleArea(Vector3 A, Vector3 B, Vector3 C)
    {
        //Debug.Log(A.x + " " + A.y + " " + A.z);
        //float trArea = 0.0f;
        //trArea = 1 / 2 * ((A.x - C.x) * (B.y - C.y) - (A.y - C.y)* (B.x - C.x));

        float AB = ((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y) + (A.z - B.z) * (A.z - B.z));
        float ab = Mathf.Sqrt(AB);

        float BC = ((B.x - C.x) * (B.x - C.x) + (B.y - C.y) * (B.y - C.y) + (B.z - C.z) * (B.z - C.z));
        float bc = Mathf.Sqrt(BC);

        float AC = ((A.x - C.x) * (A.x - C.x) + (A.y - C.y) * (A.y - C.y) + (A.z - C.z) * (A.z - C.z));
        float ac = Mathf.Sqrt(AC);

        float p = ((ab + ac + bc) / 3);

        float S = ((p - AB) * (p - AC) * (p - BC) * p);

        float s = Mathf.Sqrt(S);

       // Debug.Log(s);
        return s;
    }
}
