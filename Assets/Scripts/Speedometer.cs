using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.rotateZ(this.gameObject, 90, 2).setEaseInCirc().setEaseOutElastic();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void rotateTo(float degrees) {
        LeanTween.cancel(this.gameObject);
        LeanTween.rotateZ(this.gameObject, degrees, 1).setEaseInCirc().setEaseOutElastic();
    }

    public void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
}
