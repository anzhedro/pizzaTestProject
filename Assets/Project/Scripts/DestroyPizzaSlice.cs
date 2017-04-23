using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DestroyPizzaSlice : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 3.0f);
    }
}
