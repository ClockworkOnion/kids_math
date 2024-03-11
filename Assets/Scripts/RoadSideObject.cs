using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSideObject : MonoBehaviour
{
    StageManager stageManager;
    RoadSideObjectAggregator aggregator;
    bool isMoving = true;
    public bool popUp = false;
    public float jumpDelay = -20f;
    private float jumpLocation = 200f;

    private void Awake()
    {
    }

    private void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageManager.stageStart.AddListener(OnStageStart);
        stageManager.stageEnd.AddListener(OnStageEnd);
        aggregator = GameObject.Find("StageDeco").GetComponent<RoadSideObjectAggregator>();
        aggregator.SetMaxZ(transform.position);
    }

    public void OnStageStart()
    {
        jumpLocation = aggregator.GetMaxZ();
        isMoving = true;
    }

    public void OnStageEnd()
    {
        isMoving = false;
    }

    private void FixedUpdate()
    {
        if (isMoving)
            MoveObject();
    }

    private void MoveObject()
    {
        float speed = stageManager.getDriveSpeed();
        const float FACTOR = 10;
        Vector3 worldForward = new(0, 0, -1);
        float movement = FACTOR * speed * Time.fixedDeltaTime;
        transform.position += movement * worldForward;

        // Jump back
        if (transform.position.z <= jumpDelay)
        {
            transform.position += new Vector3(0, 0, jumpLocation);
            if (popUp)
            {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), .5f).setEaseOutElastic();
            }
        }
    }
}
