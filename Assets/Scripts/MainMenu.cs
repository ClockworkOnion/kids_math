using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public int menuItemOffset = 80;
    string[] gameModes = { "Addition", "Find the largest" };
    List<GameObject> menuTextObjects = new List<GameObject>();

    private void Awake()
    {
        // Create child objects for game modes
        int index = 0;
        foreach (string mode in gameModes)
        {
            // Create the object and add a TextMeshProUGUI
            GameObject textObject = new GameObject(mode, typeof(RectTransform));
            textObject.transform.SetParent(transform, false);
            TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.SetText((index + 1).ToString() + ". " + mode);

            // Set the position
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchoredPosition += new Vector2(0, -menuItemOffset * index);

            // Remember the reference in List
            menuTextObjects.Add(textObject);

            index++;
        }
    }

    void Update()
    {
        bool pressedOne = Input.GetKeyDown("1");
        bool pressedTwo = Input.GetKeyDown("2");

        if (pressedOne)
        {
            GameManager.GetInstance().SetGameMode(StageManager.GameMode.equationLvl1);
            SceneManager.LoadScene("EasyModeScene");
        }

        if (pressedTwo)
        {
            GameManager.GetInstance().SetGameMode(StageManager.GameMode.findBiggest);
            SceneManager.LoadScene("EasyModeScene");
        }

    }
}
