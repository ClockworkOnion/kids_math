using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public int menuItemOffset = 80;
    public TMP_FontAsset menuFont;
    private Camera mainCamera;
    private TextMeshProUGUI playerCoinsText;

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
