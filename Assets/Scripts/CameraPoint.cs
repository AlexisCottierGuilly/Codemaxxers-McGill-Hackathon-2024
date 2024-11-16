using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public GameObject target;
    public float speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(target.transform);
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
}
