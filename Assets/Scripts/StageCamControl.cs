using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCamControl : MonoBehaviour
{
    public Transform upperCameraPosition;
    public Transform zoomOutCameraPosition;
    private Vector3 baseCamPos;
    private Vector3 baseCamRot;

    private StageManager stageManager;

    private void Awake()
    {
        baseCamPos = transform.position;
        baseCamRot = transform.rotation.eulerAngles;
    }

    private void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageManager.stageEnd.AddListener(OnStageEnd);
        stageManager.stageStart.AddListener(OnStageStart);
        stageManager.zoomOut.AddListener(OnFadeOut);

    }

    private void OnStageEnd()
    {
        TweenCameraTo(upperCameraPosition, 3f);
    }

    private void OnStageStart()
    {
        TweenCameraTo(baseCamPos, baseCamRot, 1f);
    }

    private void OnFadeOut() {
        TweenCameraTo(zoomOutCameraPosition, 1.5f);
    }

    public void TweenCameraTo(Transform newPosition, float time)
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, newPosition, time).setEaseInOutCubic();
        LeanTween.rotate(gameObject, newPosition.rotation.eulerAngles, time).setEaseInOutCubic();
    }

    public void TweenCameraTo(Vector3 newPosition, Vector3 newRotation, float time)
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, newPosition, time).setEaseInOutCubic();
        LeanTween.rotate(gameObject, newRotation, time).setEaseInOutCubic();
    }
}
