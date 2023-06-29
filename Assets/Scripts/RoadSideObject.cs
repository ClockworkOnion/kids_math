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

        Vector3 worldForward = new Vector3(0, 0, -1);
        transform.position += worldForward * speed * Time.fixedDeltaTime;

        if (transform.position.z <= -20) { transform.position += new Vector3(0, 0, 200); }
    }
}
