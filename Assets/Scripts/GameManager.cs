using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static StageManager.GameMode currentMode;

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

        #region Debug keys region...
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

        // Debug reset stage progression
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.P))
        {
            PlayerPrefs.SetInt("stageProgression", 0);
            Debug.Log("Resetting stage progression...");
        }

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.Alpha1))
        {
            PlayerPrefs.SetInt("stageProgression", 1);
        }

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.Alpha2))
        {
            PlayerPrefs.SetInt("stageProgression", 2);
        }

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.Alpha3))
        {
            PlayerPrefs.SetInt("stageProgression", 3);
        }
        // End of debug keys code region
        #endregion


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

    public List<StageManager.GameMode> stageProgression = new List<StageManager.GameMode> {
        StageManager.GameMode.addLv1,
        StageManager.GameMode.subLv1,
        StageManager.GameMode.addSubLv1,

        StageManager.GameMode.addLv2,
        StageManager.GameMode.subLv2,
        StageManager.GameMode.addSubLv2,

        StageManager.GameMode.addLv3,
        StageManager.GameMode.subLv3,
        StageManager.GameMode.addSubLv3,

        StageManager.GameMode.addLv4,
        StageManager.GameMode.subLv4,
        StageManager.GameMode.addSubLv4,

        StageManager.GameMode.multLv1,
        StageManager.GameMode.divLv1,
        StageManager.GameMode.multDivLv1,

        StageManager.GameMode.multLv2,
        StageManager.GameMode.divLv2,
        StageManager.GameMode.multDivLv2,

        StageManager.GameMode.multLv3,
        StageManager.GameMode.divLv3,
        StageManager.GameMode.multDivLv3,

        StageManager.GameMode.multLv4,
        StageManager.GameMode.divLv4,
        StageManager.GameMode.multDivLv4,

        StageManager.GameMode.addSubMultDivLv1,
        StageManager.GameMode.addSubMultDivLv2,
        StageManager.GameMode.addSubMultDivLv3,
        StageManager.GameMode.addSubMultDivLv4,
    };
}
