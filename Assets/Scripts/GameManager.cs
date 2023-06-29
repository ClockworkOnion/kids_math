using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager instance;
    static StageManager.GameMode currentMode;

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
            Destroy(this.gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameMode(StageManager.GameMode newMode) {
        currentMode = newMode;
    }

    public StageManager.GameMode GetGameMode() {
        return currentMode;
    }
}
