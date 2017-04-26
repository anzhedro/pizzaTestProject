using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PizzaAreaHandle : MonoBehaviour
{

    float fullPizzaArea = 0.0f;
    float pizzaArea = 0.0f;
    float prevPizzaArea = 0.0f;
    float pizzaSliceArea = 0.0f;
    int cutsCount = 0;
    public GameObject Pizza;
    public Text pizzaAreaText;
    public Text pizzaSliceAreaText;
    public Text cutsCountText;
    Mesh mesh;
    
    void Update()
    {
        if (CutPizza.buttonPress)
        {
            mesh = Pizza.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;

            fullPizzaArea = BuildPizza.totalPizzaArea;

            float area = 0;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                float trArea = calcTriangleArea(vertices[mesh.triangles[i]], vertices[mesh.triangles[i + 1]], vertices[mesh.triangles[i + 2]]);
                if (!System.Single.IsNaN(trArea))
                {
                    area += trArea;
                }
            }
            prevPizzaArea = pizzaArea;
            pizzaArea = area;
            pizzaAreaText.text = "Pizza area: " + ((pizzaArea / fullPizzaArea) * 100) + "%";

            if (cutsCount == 0)
            {
                pizzaSliceArea = (((fullPizzaArea - pizzaArea) / fullPizzaArea) * 100);
            }
            else
            {
                pizzaSliceArea = (((prevPizzaArea - pizzaArea) / fullPizzaArea) * 100);                
            }

            pizzaSliceAreaText.text = "Slice area: " + pizzaSliceArea + "%";
            cutsCount++;
            cutsCountText.text = "Cuts count: " + cutsCount;

            CutPizza.buttonPress = false;
        }
    }

    public static float calcTriangleArea(Vector3 A, Vector3 B, Vector3 C)
    {
        float AB = ((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y));
        float ab = Mathf.Sqrt(AB);

        float BC = ((B.x - C.x) * (B.x - C.x) + (B.y - C.y) * (B.y - C.y));
        float bc = Mathf.Sqrt(BC);

        float AC = ((A.x - C.x) * (A.x - C.x) + (A.y - C.y) * (A.y - C.y));
        float ac = Mathf.Sqrt(AC);

        float p = ((ab + ac + bc) / 3);

        float S = ((p - AB) * (p - AC) * (p - BC) * p);

        float s = Mathf.Sqrt(S);

        // Debug.Log(s);
        return s;
    }
}
