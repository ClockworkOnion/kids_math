using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageUI : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 onScreenPosition;
    private Vector3 offScreenPosition;
    private bool hidden = true;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        offScreenPosition = rect.anchoredPosition;
        onScreenPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show() { 
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, onScreenPosition, 1.4f).setEaseOutBounce();
    }

    public void Hide() { 
        LeanTween.cancel(gameObject);
        LeanTween.move(rect, offScreenPosition, 1.4f).setEaseInCirc();
    }

    public void ToggleShowHide() {
        hidden = !hidden;
        if (hidden) Hide();
        if (!hidden) Show();
    }
}
