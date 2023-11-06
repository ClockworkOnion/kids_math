using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    static StageManager.GameMode currentMode;

    public Material carMaterial { get; private set; }

    public static GameManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.LoadScene("MainMenuScene");

        }
        else
        {
            Debug.Log("Previous Game Manager already existed! Destroying...");
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        // Debug Reset Colors key combo and print money TODO delete and remove TMPro namespace TODO
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.M))
            ResetColors();

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.M))
            AddPlayerMoney(10);
        GameObject coinses = GameObject.Find("CoinsText");
        if (coinses)
        {
            TextMeshProUGUI CoinsText = GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>();
            CoinsText?.SetText("Coins: " + PlayerPrefs.GetInt("player_money"));
        }

    }

    public void SetGameMode(StageManager.GameMode newMode)
    {
        currentMode = newMode;
    }

    public StageManager.GameMode GetGameMode()
    {
        return currentMode;
    }

    public void SetCarMaterial(Material mat)
    {
        Debug.Log("Setting material to " + mat);
        carMaterial = mat;
        GameObject.Find("new_car").GetComponent<CarCustomizer>().SwitchMaterial(mat);
    }

    public void AddPlayerMoney(int amount)
    {
        int currentAmount = PlayerPrefs.GetInt("player_money");
        PlayerPrefs.SetInt("player_money", currentAmount + amount);
    }

    public void UnlockColor(string color)
    {
        PlayerPrefs.SetInt(color, 1);
    }

    public void ResetColors()
    {
        PlayerPrefs.SetInt("BlueMat", 0);
        PlayerPrefs.SetInt("RedMat", 0);
        PlayerPrefs.SetInt("YellowMat", 0);
        PlayerPrefs.SetInt("GreenMat", 0);
        PlayerPrefs.SetInt("PurpleMat", 0);
        PlayerPrefs.SetInt("WhiteMat", 0);
        PlayerPrefs.SetInt("PinkMat", 0);
        PlayerPrefs.SetInt("BlackMat", 0);
    }
}
