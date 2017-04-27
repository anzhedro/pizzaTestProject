using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    float radius = 1.1f;
    private float dist;
    public static bool dragging = false;
    public static bool touchDragging = false;

    private Transform toDrag;
    Vector3 v3;
    Touch touch;
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable"))
            {
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                toDrag = hit.transform;
                dragging = true;
            }
        }
        if (dragging)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);

            v3.Normalize();
            Vector3 rad = v3 * radius;
            toDrag.position = rad;
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
        }



        if (Input.touchCount != 1)
        {
            touchDragging = false;
            return;
        }
        touch = Input.touches[0];


        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("B");
            Vector3 position = touch.position;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(position);


            if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable"))
            {
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                toDrag = hit.transform;
                touchDragging = true;
            }
        }
        if (touchDragging && touch.phase == TouchPhase.Moved)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);

            v3.Normalize();
            Vector3 rad = v3 * radius;
            toDrag.position = rad;
        }
        if (touchDragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            touchDragging = false;
        }
    }
}
