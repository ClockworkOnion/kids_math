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
        TweenCameraTo(camPositions[level - 1]);
    }

    public void ZoomOutFrom(int zoomPosition)
    {
        SetCameraTo(zoomedInPositions[zoomPosition]);
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

    public void StartAtLevel(int level)
    {
        const float setupTime = 1f;
        const float zoomTime = 1f;
        float distanceToLevelViewPos = Vector3.Distance(transform.position, camPositions[level].transform.position);

        LeanTween.cancel(gameObject);
        if (distanceToLevelViewPos < 0.1f) // Skip setup if already at position
        {
            FadeToWhite(zoomTime);
            ZoomAndGo();
        }
        else
        {
            LeanTween.rotate(gameObject, camPositions[level].transform.rotation.eulerAngles, setupTime).setEaseInOutCubic();
            LeanTween.move(gameObject, camPositions[level], setupTime).setEaseInOutCubic().setOnComplete(() =>
            {
                FadeToWhite(zoomTime);
                ZoomAndGo();
            });
        }

        void ZoomAndGo()
        {
            LeanTween.rotate(gameObject, zoomedInPositions[level].transform.rotation.eulerAngles, zoomTime).setEaseInOutCubic();
            LeanTween.move(gameObject, zoomedInPositions[level], zoomTime).setEaseInOutCubic().setOnComplete(() =>
                SceneManager.LoadScene("EasyModeScene"));
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
