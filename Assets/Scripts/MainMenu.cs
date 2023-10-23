using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public int menuItemOffset = 80;
    public TMP_FontAsset menuFont;
    string[] gameModes = { "Addition Level 1", "Addition Level 2", "Addition Level 3" };
    List<GameObject> menuTextObjects = new List<GameObject>();
    private Camera mainCamera;

    public GameObject mobileCheckmark;

    private void Awake()
    {
        //// Create child objects for game modes
        //int index = 0;
        //foreach (string mode in gameModes)
        //{
        //    // Create the object and add a TextMeshProUGUI
        //    GameObject textObject = new GameObject(mode, typeof(RectTransform));
        //    textObject.transform.SetParent(transform, false);
        //    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        //    textComponent.SetText((index + 1).ToString() + ". " + mode);
        //    if (menuFont) textComponent.font = menuFont;
        //    textComponent.color = new Color32(245, 245, 121, 255);

        //    // Set the position
        //    RectTransform rect = textObject.GetComponent<RectTransform>();
        //    rect.anchoredPosition += new Vector2(0, -menuItemOffset * index);

        //    // Remember the reference in List
        //    menuTextObjects.Add(textObject);

        //    index++;
        //}

        if (!Application.isMobilePlatform)
        {
            Debug.Log("Platform isn't mobile. Removing checkmark...");
            Destroy(mobileCheckmark);
        }
    }


    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CameraWobbleUp();
    }

    void Update()
    {
        bool pressedOne = Input.GetKeyDown("1");
        bool pressedTwo = Input.GetKeyDown("2");
        bool pressedThree = Input.GetKeyDown("3");
        //bool pressedFour = Input.GetKeyDown("4");

        if (pressedOne)
        {
            GameManager.GetInstance().SetGameMode(StageManager.GameMode.addLv1);
            SceneManager.LoadScene("EasyModeScene");
        }

        if (pressedTwo)
        {
            GameManager.GetInstance().SetGameMode(StageManager.GameMode.addLv2);
            SceneManager.LoadScene("EasyModeScene");
        }

        if (pressedThree)
        {
            GameManager.GetInstance().SetGameMode(StageManager.GameMode.addLv3);
            SceneManager.LoadScene("EasyModeScene");
        }
    }

    public void StartGame(int level)
    {
        switch (level)
        {
            case 1:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.addLv1);
                break;
            case 2:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.addLv2);
                break;
            case 3:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.addLv3);
                break;
            default:
                break;
        }
        SceneManager.LoadScene("EasyModeScene");
    }

    private void CameraWobbleUp()
    {
        LeanTween.move(mainCamera.gameObject, mainCamera.transform.position + Vector3.up * 2, 30).setEaseInOutSine().setOnComplete(CameraWobbleDown);
    }

    private void CameraWobbleDown()
    {
        LeanTween.move(mainCamera.gameObject, mainCamera.transform.position + Vector3.up * -2, 30).setEaseInOutSine().setOnComplete(CameraWobbleUp);
    }

}
