using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectButton : MonoBehaviour
{
    public Material toApply;

    public void OnButtonClick() {
        GameManager.GetInstance().SetCarMaterial(toApply);
    }
}
