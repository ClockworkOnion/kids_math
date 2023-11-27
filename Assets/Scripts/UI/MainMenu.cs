using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public int menuItemOffset = 80;
    public TMP_FontAsset menuFont;
    private Camera mainCamera;
    private TextMeshProUGUI playerCoinsText;

    public TextMeshProUGUI stageProgressionIndicator;
    public GameObject mobileCheckmark;

    private void Awake()
    {
        playerCoinsText = GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>();
        playerCoinsText.SetText("Coins: " + PlayerPrefs.GetInt("player_money").ToString());

        if (!Application.isMobilePlatform)
        {
            Debug.Log("Platform isn't mobile. Removing checkmark...");
            Destroy(mobileCheckmark);
        }

        int stageProgress = PlayerPrefs.GetInt("stageProgression");
        stageProgressionIndicator.text = "Stage: " + (stageProgress+1).ToString() +
            "\nType: " + GameManager.GetInstance().stageProgression[stageProgress].ToString();
    }

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CameraWobbleUp();
    }

    void Update()
    {
        if (Input.GetKeyDown("1")) StartGame(1);
        else if (Input.GetKeyDown("2")) StartGame(2);
        else if (Input.GetKeyDown("3")) StartGame(3);
        else if (Input.GetKeyDown("4")) StartGame(4);
        else if (Input.GetKeyDown("5")) StartGame(5);
        else if (Input.GetKeyDown("6")) StartGame(6);
        else if (Input.GetKeyDown("7")) StartGame(7);
        else if (Input.GetKeyDown("8")) StartGame(8);
        else if (Input.GetKeyDown("9")) StartGame(9);
    }

    public void StartGame() {

        int currentProgress = Mathf.Min(GameManager.GetInstance().stageProgression.Count-1, PlayerPrefs.GetInt("stageProgression"));
        StageManager.GameMode nextGameMode = GameManager.GetInstance().stageProgression[currentProgress];
        GameManager.GetInstance().SetGameMode(nextGameMode);
        SceneManager.LoadScene("EasyModeScene");
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
            case 4:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.multLv1);
                break;
            case 5:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.multLv2);
                break;
            case 6:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.multLv3);
                break;
            case 7:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.addMultLv3);
                break;
            case 8:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.subLv1);
                break;
            case 9:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.subLv2);
                break;
            case 10:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.divLv1);
                break;
            case 11:
                GameManager.GetInstance().SetGameMode(StageManager.GameMode.divLv2);
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
