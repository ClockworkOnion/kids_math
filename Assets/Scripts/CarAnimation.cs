using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    public Transform tire_left_front;
    public Transform tire_left_rear;
    public Transform tire_right_front;
    public Transform tire_right_rear;
    public Transform car_frame;

    public float drivingSpeed = 5f;
    public Transform parent;

    private float prevX = 0f;
    private float currentX = 0f;

    private ParticleSystem particleLeft;
    private ParticleSystem particleRight;

    private StageManager stageManager;



    private void Awake()
    {
        parent = GameObject.Find("PlayerCar").GetComponent<Transform>();
        particleLeft = GameObject.Find("ParticleLeft").GetComponent<ParticleSystem>();
        particleRight = GameObject.Find("ParticleRight").GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        drivingSpeed = stageManager.getDriveSpeed() / 7;

        // Set car rotation according to movement between last frame
        float xDiff = (transform.position.x - prevX) * 100;
        Vector3 rotationAngles = new Vector3(0, 180 + xDiff, 0);
        transform.rotation = Quaternion.Euler(rotationAngles);
        prevX = transform.position.x;

    }

    private void FixedUpdate()
    {
        rotateTires();
        jitterFrame();



    }

    private void rotateTires()
    {
        tire_left_front.Rotate(Vector3.right, drivingSpeed);
        tire_left_rear.Rotate(Vector3.right, drivingSpeed);
        tire_right_front.Rotate(-Vector3.right, drivingSpeed);
        tire_right_rear.Rotate(-Vector3.right, drivingSpeed);
    }

    private void jitterFrame()
    {
        float timeStamp = Time.fixedTime;
        float displacement = (2f * Mathf.Sin(10f * timeStamp) + 3f * Mathf.Sin(2f * timeStamp) + 2f * Mathf.Sin(30f * timeStamp) + 25) * 0.01f;
        Vector3 displace_vector = new Vector3(0, displacement, 0);
        car_frame.position = parent.position + displace_vector;
    }

    public void smokeBurst()
    {
            particleLeft.Play();
            particleRight.Play();
    }
}
