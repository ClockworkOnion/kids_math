using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCamControl : MonoBehaviour
{
    void Update()
    {
        float rotationSpeed = 5f;
        transform.Rotate(new Vector3(0f, rotationSpeed, 0f) * Time.deltaTime);   
    }
}
