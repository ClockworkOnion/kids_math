using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaytimeSwitch : MonoBehaviour
{
    private Color nightLine = new Color(0.08f, 0.3f, 0.36f); // Night Line!? :D
    private Color dayLine = new Color(1f, 1f, 1f);

    private Color cameraDayBG = new Color(0.45f, 0.85f, 1f);
    private Color cameraNightBG = new Color(0.11f, 0.19f, 0.22f);

    private Vector3 dayRotation = new Vector3(50f, -30f, 0f);
    private Vector3 eveningRotation = new(7.78f, 83f, 106f);

    private Light stageLight;
    private Camera mainCam;

    private bool isNightTime = false;

    // Start is called before the first frame update
    void Start()
    {
        stageLight = GetComponent<Light>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (isNightTime)
                ToDayTime();
            else
                ToNightTime();
        }
    }

        private void ToNightTime()
        {
            isNightTime = true;
            mainCam.backgroundColor = cameraNightBG;
            stageLight.color = nightLine;
            stageLight.transform.rotation = Quaternion.Euler(eveningRotation);
        }

        private void ToDayTime()
        {
            isNightTime = false;
            mainCam.backgroundColor = cameraDayBG;
            stageLight.color = dayLine;
            stageLight.transform.rotation = Quaternion.Euler(eveningRotation);
        }
    }
