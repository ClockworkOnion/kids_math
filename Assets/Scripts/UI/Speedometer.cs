using UnityEngine;

public class Speedometer : MonoBehaviour
{
    void Start()
    {
        LeanTween.rotateZ(gameObject, 90, 2).setEaseInCirc().setEaseOutElastic();
    }

    public void rotateTo(float degrees) {
        LeanTween.cancel(gameObject);
        LeanTween.rotateZ(gameObject, degrees, 1).setEaseInCirc().setEaseOutElastic();
    }

    public void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
}
