using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuTouch : MonoBehaviour
{

    private TextMeshProUGUI sensorReadings;
    private TextMeshProUGUI statusMsg;
    private int counter = 0;

    private void Awake()
    {
        sensorReadings = GameObject.Find("SensorReadings").GetComponent<TextMeshProUGUI>();
        statusMsg = GameObject.Find("StatusMsg").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
    }

    void Update()
    {
        //string isMobile = Application.isMobilePlatform.ToString();
        //string touchPos = Input.GetTouch(0).ToString();
        //sensorReadings.text = "isMobile: " + isMobile + "\nTouch: " + touchPos;
    }

    public void onButton1Press() {
        Debug.Log("Button 1 pressed");
        counter++;
        statusMsg.text = counter.ToString();
    }
}
