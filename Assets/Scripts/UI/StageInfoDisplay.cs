using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageInfoDisplay : MonoBehaviour
{
    private TextMeshProUGUI stageInfo;
    private TextMeshProUGUI playButtonText;
    
    private string[] stageDescriptions = { "Town\nAddition Level 1", "Desert", "City" };
    void Start()
    {
        stageInfo = GameObject.Find("StageDescription").GetComponent<TextMeshProUGUI>();
        playButtonText = GameObject.Find("ButtonDescription").GetComponent<TextMeshProUGUI>();

        LevelSelectSphere[] spheres = GameObject.Find("scenery").GetComponentsInChildren<LevelSelectSphere>();
        foreach (LevelSelectSphere s in spheres)
            s.sphereClicked.AddListener(SetStageNumber);

    }

    private void SetStageNumber(int number) {
        stageInfo.text = (number > stageDescriptions.Length) ? "Stage name missing" : (stageDescriptions[number - 1]);
        int prog = PlayerPrefs.GetInt("stageProgression");
        playButtonText.text = (prog >= number) ? "Replay now!" : "Locked!";
    }
}
