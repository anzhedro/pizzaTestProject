using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    //public GameObject cylinder;
    private Vector3 screenPoint;
    private Vector3 offset;
    float radius = 1.1f;
    private float dist;
    private bool dragging = false;
    // private Vector3 offset;
    private Transform toDrag;
    Vector3 v3;
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
            Vector3 mid = new Vector3(0, 0, 0);
            toDrag.position = rad;


            Vector3 offset = rad - mid;
            Vector3 position = mid + (offset / 2.0f);
            GameObject cylinder = toDrag.GetChild(0).gameObject;
            cylinder.transform.position = position;
            cylinder.transform.up = offset;
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
        }


        //Vector3 v3;

        //if (Input.touchCount != 1)
        //{
        //    dragging = false;
        //    return;
        //}

        //Touch touch = Input.touches[0];
        //Vector3 pos = touch.position;

        //if (touch.phase == TouchPhase.Began)
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(pos);
        //    if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable"))
        //    {
        //        Debug.Log("Here");
        //        toDrag = hit.transform;
        //        dist = hit.transform.position.z - Camera.main.transform.position.z;
        //        v3 = new Vector3(pos.x, pos.y, dist);
        //        v3 = Camera.main.ScreenToWorldPoint(v3);
        //        offset = toDrag.position - v3;
        //        dragging = true;
        //    }
        //}
        //if (dragging && touch.phase == TouchPhase.Moved)
        //{
        //    v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        //    v3 = Camera.main.ScreenToWorldPoint(v3);
        //    toDrag.position = v3 + offset;
        //}
        //if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        //{
        //    dragging = false;
        //}
    }
}
