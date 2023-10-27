using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSideObject : MonoBehaviour
{
    StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    private void FixedUpdate()
    {
        float speed = stageManager.getDriveSpeed();
        const float FACTOR = 10;
        Vector3 worldForward = new(0, 0, -1);
        float movement = FACTOR * speed * Time.fixedDeltaTime;
        transform.position += movement * worldForward;

        // Jump back
        if (transform.position.z <= -20) 
	        transform.position += new Vector3(0, 0, 200);
    }
}
