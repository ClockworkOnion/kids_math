using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCamControl : MonoBehaviour
{
    public LevelSelectSphere[] levelSpheres;
    public Transform[] camPositions;
    public Transform[] zoomedInPositions;
    public Transform lightSource;
    public CanvasGroup whiteFader;

    void Start()
    {
        foreach (LevelSelectSphere sphere in levelSpheres)
        {
            sphere.sphereClicked.AddListener(SelectLevel);
        }
    }

    public void SelectLevel(int level)
    {
        Debug.Log("Selecting Lv " + level);
        TweenCameraTo(camPositions[(level - 1) % camPositions.Length] );
    }

    public void ZoomOutFrom(int zoomPosition)
    {
        SetCameraTo(zoomedInPositions[zoomPosition % zoomedInPositions.Length]);
        SelectLevel(zoomPosition + 1);
    }

    public void TweenCameraTo(Transform newPosition)
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, newPosition, 1f).setEaseInOutCubic();
        LeanTween.rotate(gameObject, newPosition.rotation.eulerAngles, 1f).setEaseInOutCubic();

        // rotate lightsource
        LeanTween.rotate(lightSource.gameObject, newPosition.rotation.eulerAngles, 1f).setEaseInOutCubic();
    }

    public void SetCameraTo(Transform newPosition)
    {
        LeanTween.cancel(gameObject);
        transform.position = newPosition.position;
        transform.rotation = newPosition.rotation;

        // Lightsource
        lightSource.transform.rotation = newPosition.rotation;
    }

    public void StartAtLevel(int levelNumber, ModeData.StageScenes scene)
    {
        const float setupTime = 1f;
        const float zoomTime = 1f;
        float distanceToLevelViewPos = Vector3.Distance(transform.position, camPositions[levelNumber].transform.position);

        LeanTween.cancel(gameObject);
        if (distanceToLevelViewPos < 0.1f) // Skip setup if already at position
        {
            FadeToWhite(zoomTime);
            ZoomAndGo(scene);
        }
        else
        {
            LeanTween.rotate(gameObject, camPositions[levelNumber].transform.rotation.eulerAngles, setupTime).setEaseInOutCubic();
            LeanTween.move(gameObject, camPositions[levelNumber], setupTime).setEaseInOutCubic().setOnComplete(() =>
            {
                FadeToWhite(zoomTime);
                ZoomAndGo(scene);
            });
        }

        void ZoomAndGo(ModeData.StageScenes sceneToLoad)
        {
            string sceneString = ModeData.stageNames[sceneToLoad];
            LeanTween.rotate(gameObject, zoomedInPositions[levelNumber].transform.rotation.eulerAngles, zoomTime).setEaseInOutCubic();
            LeanTween.move(gameObject, zoomedInPositions[levelNumber], zoomTime).setEaseInOutCubic().setOnComplete(() =>
                SceneManager.LoadScene(sceneString));
        }
    }

    public void FadeToWhite(float fadeTime)
    {
        LeanTween.value(gameObject, UpdateFloat, 0f, 1f, fadeTime).setEaseInCubic();

        void UpdateFloat(float alpha)
        {
            whiteFader.alpha = alpha;
        }
    }

}
