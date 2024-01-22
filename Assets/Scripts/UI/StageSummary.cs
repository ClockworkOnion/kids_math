using UnityEngine;
using TMPro;

/// <summary>
/// On the UI element for displaying the summary at the end of the stage. Controls the number of
/// green circles and red x's that appear, as well as the amount of coin gained, and should handle
/// returning to the map or replaying the stage.
/// </summary>
public class StageSummary : MonoBehaviour
{
    // Assign in inspector:
    public GameObject GreenCircleParent;
    public GameObject RedCrossParent;
    public GameObject GreenCirclePrefab;
    public GameObject RedCrossPrefab;
    public TextMeshProUGUI coinsText;
    private CanvasGroup canvasGroup;

    private Vector3 offScreenPosition;
    private Vector3 onScreenPosition;
    private RectTransform rect;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        onScreenPosition = rect.anchoredPosition;
        offScreenPosition = new Vector3(onScreenPosition.x * -1, onScreenPosition.y, onScreenPosition.z);
        rect.anchoredPosition = offScreenPosition;
    }

    public void ShowScores(int correctCount, int incorrectCount, int coinsGained)
    {
        for (int i = 0; i < correctCount; i++)
        {
            GameObject circle = Instantiate(GreenCirclePrefab);
            circle.transform.SetParent(GreenCircleParent.transform);
            circle.transform.localScale = new Vector3(1, 1, 1);
        }

        for (int i = 0; i < incorrectCount; i++)
        {
            GameObject cross = Instantiate(RedCrossPrefab);
            cross.transform.SetParent(RedCrossParent.transform);
            cross.transform.localScale = new Vector3(1, 1, 1);
        }

        coinsText.SetText("You got " + coinsGained + " coins!");
    }

    public void Show()
    {
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, onScreenPosition, 1.4f).setEaseOutBounce();
    }

    public void Hide()
    {
        float opaqueAlpha = 1f;
        float transparentAlpha = 0f;
        float duration = .5f;
        LeanTween.value(gameObject, SetAlpha, opaqueAlpha, transparentAlpha, duration).setOnComplete(() =>
        {
            rect.anchoredPosition = offScreenPosition;
            SetAlpha(1);
        });

    }

    public void SetAlpha(float newValue)
    {
        canvasGroup.alpha = newValue;
    }


}
