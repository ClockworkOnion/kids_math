using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    static StageManager.GameMode currentMode;

    public Material carMaterial { get; private set; }

    public static GameManager GetInstance() {
        return instance;
    }

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.LoadScene("MainMenuScene");

        } else {
            Debug.Log("Previous Game Manager already existed! Destroying...");
            Destroy(gameObject);
            return;
        }
    }

    public void SetGameMode(StageManager.GameMode newMode) {
        currentMode = newMode;
    }

    public StageManager.GameMode GetGameMode() {
        return currentMode;
    }

    public void SetCarMaterial(Material mat) {
        Debug.Log("Setting material to " + mat);
        carMaterial = mat;
        GameObject.Find("new_car").GetComponent<CarCustomizer>().SwitchMaterial(mat);
    }
}
