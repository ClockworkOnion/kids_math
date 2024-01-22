using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSideObject : MonoBehaviour
{
    StageManager stageManager;
    bool isMoving = true;

    private void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageManager.stageStart.AddListener(OnStageStart);
        stageManager.stageEnd.AddListener(OnStageEnd);
    }

    public void OnStageStart() {
        isMoving = true;
    }

    public void OnStageEnd() {
        isMoving = false;
    }

    private void FixedUpdate()
    {
        if (isMoving)
            MoveObject();
    }

    private void MoveObject() { 
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
