using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILetter : MonoBehaviour
{
    private float timeScaleOffset = 0f;

    private float wobbleSpeedFactor = 1f;
    private float wobbleIntensity = 0.7f;



    private void Awake()
    {
        timeScaleOffset = Random.Range(0, 180);
        wobbleIntensity = Random.Range(0.05f, 0.1f);
        wobbleSpeedFactor = Random.Range(3f, 5f);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float wobble = Mathf.Sin(wobbleSpeedFactor * Time.time + timeScaleOffset) * wobbleIntensity;
        float wobble2 = Mathf.Sin(wobbleSpeedFactor * Time.time + timeScaleOffset* 1.35f) * wobbleIntensity;
        transform.position += new Vector3(wobble2, wobble, 0);

        transform.Rotate(new Vector3(0, 0, wobble));
        //TODO: Use LeanTween to push the letters to positions/rotations, instead of updating position here every frame
    }
}
