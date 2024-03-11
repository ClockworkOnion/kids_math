using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private Scene lastLoaded;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Loaded scene: " + scene.name);
        switch (scene.name)
        {
            case "MainMenuScene":
                MenuSceneConstructor();
                break;
            default:
                break;
        }
        lastLoaded = scene;
    }

    public void MenuSceneConstructor()
    {
        StageManager.GameMode previousMode = GameManager.currentMode;
        MainMenuCamControl camControl = Camera.main.GetComponent<MainMenuCamControl>();

        Debug.Log("stageProgression: " + PlayerPrefs.GetInt("stageProgression"));
        camControl.ZoomOutFrom(PlayerPrefs.GetInt("stageProgression") - 1); // Zoom out from previous stage

        CanvasGroup whiteFader = GameObject.Find("WhiteFade").GetComponent<CanvasGroup>();
        if (lastLoaded != null)
        {
            // Fade out the white overlay
            LeanTween.value(gameObject, UpdateFloat, 1f, 0f, 1f).setEaseInCubic();
            void UpdateFloat(float alpha)
            {
                whiteFader.alpha = alpha;
            }

            //camControl.transform.position = 
        }
        else
        {
            whiteFader.alpha = 0;
        }

    }

    public void StageSceneConstructor()
    {

    }

}
