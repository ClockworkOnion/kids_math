using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplayManager : MonoBehaviour
{

    public TextElement equationLeft;
    public TextElement equationCenter;
    public TextElement equationRight;
    public TextMeshProUGUI statusText;

    public void SetEquationText(TextPos position, string displayText) {
        switch (position)
        {
            case TextPos.left:
                equationLeft.SetText(displayText);
                break;
            case TextPos.right:
                equationRight.SetText(displayText);
                break;
            case TextPos.center:
                equationCenter.SetText(displayText);
                break;
            default:
                break;
        }
    }


    public void SetStatusText(string text) {
        statusText.SetText(text);
    }

    public void ClearDisplay() {
        equationLeft.ClearChildren();
        equationRight.ClearChildren();
        equationCenter.ClearChildren();
    }

    public enum TextPos { 
        left, right, center
    }
}
