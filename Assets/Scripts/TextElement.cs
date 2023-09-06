using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextElement : MonoBehaviour
{
    private string currentText;

    private float letterOffset = 20f;

    public string GetText() {
        if (currentText == null) return "";
        return currentText;
    }

    public void SetText(string toPrint) {
        currentText = toPrint;
        int index = 0;
        foreach (char c in toPrint)
        {
            // Create the character and set its string
            GameObject obj = new GameObject(c.ToString(), typeof(RectTransform));
            obj.transform.SetParent(transform, false);
            TextMeshProUGUI objText = obj.AddComponent<TextMeshProUGUI>();
            objText.SetText(c.ToString());

            // Set the position
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchoredPosition += new Vector2(letterOffset*index, 1);

            // Set text options (color, size etc)
            TMP_ColorGradient colors = ScriptableObject.CreateInstance<TMP_ColorGradient>();
            colors.bottomLeft = RandomColor();
            colors.topLeft = RandomColor();
            colors.bottomRight = RandomColor();
            colors.topRight = RandomColor();

            objText.colorGradientPreset = colors;
            objText.enableVertexGradient = true;
            objText.fontSize = 50f;
            objText.alignment = TextAlignmentOptions.Center;

            // Add the UILetter Component
            UILetter letterScript = obj.AddComponent<UILetter>();

            index++;
        }
    }

    public void ClearChildren()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public Color RandomColor() {
        int index = Random.Range(1, 8);
        switch (index)
        {
            case 1: return Color.blue;
            case 2: return Color.cyan;
            case 3: return Color.green;
            case 4: return Color.magenta;
            case 5: return Color.red;
            case 6: return Color.yellow;
            case 7: return Color.white;
            default: return Color.white;
        }
    }
}
