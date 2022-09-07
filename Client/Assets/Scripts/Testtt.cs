using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testtt : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);
    }
}
