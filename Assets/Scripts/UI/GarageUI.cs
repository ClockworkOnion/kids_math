using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GarageUI : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 onScreenPosition;
    private Vector3 offScreenPosition;
    private bool hidden = true;

    public RectTransform startStageButton;
    private Vector3 startStageInitialPos;
    private Vector3 startStageHiddenPos;

    public TextMeshProUGUI goToGarageText;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        offScreenPosition = rect.anchoredPosition;
        onScreenPosition = new Vector3(0, -rect.anchoredPosition.y, 0);

    }

    private void Start()
    {
        startStageInitialPos = startStageButton.anchoredPosition;
        startStageHiddenPos = new Vector3(-startStageInitialPos.x, -startStageInitialPos.y, 0);
    }

    public void Show() { 
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, onScreenPosition, 1.4f).setEaseOutBounce();

        LeanTween.cancel(startStageButton.gameObject);
        LeanTween.move(startStageButton, startStageHiddenPos, 1f).setEaseInOutCirc();
        goToGarageText.SetText("Exit Garage");
    }

    public void Hide() { 
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, offScreenPosition, 1.4f).setEaseInCirc();

        LeanTween.cancel(startStageButton.gameObject);
        LeanTween.move(startStageButton, startStageInitialPos, 1f).setEaseInOutCirc();

        goToGarageText.SetText("go to Garage");
    }

    public void ToggleShowHide() {
        hidden = !hidden;
        if (hidden) Hide();
        if (!hidden) Show();
    }
}
