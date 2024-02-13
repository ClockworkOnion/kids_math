using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCamControl : MonoBehaviour
{
    public float rotationSpeed = 10f;
    void Update()
    {
        transform.Rotate(new Vector3(0f, rotationSpeed, 0f) * Time.deltaTime);   
    }
}
