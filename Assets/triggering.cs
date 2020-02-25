using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggering : MonoBehaviour
{
    void OnTriggerEnter()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
}
