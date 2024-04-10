using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Image))]
public class PrettyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private AudioSource audio;
    private Image buttonGraphic;
    private bool isHidden = false;
    private TextMeshProUGUI text;

    // Fill in inspector
    public AudioClip pressButton;
    public AudioClip releaseButton;
    public Sprite buttonNormal;
    public Sprite buttonPressed;
    public UnityEvent onClick;
    public bool activateOnRelease = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        buttonGraphic = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audio.PlayOneShot(pressButton);
        buttonGraphic.sprite = buttonPressed;

        if (!activateOnRelease)
	        onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        audio.PlayOneShot(releaseButton);
        buttonGraphic.sprite = buttonNormal;

        if (activateOnRelease)
	        onClick.Invoke();
    }

    private void FadeOut() {
        buttonGraphic.color = new Color(1, 1, 1, 0);
    }

    private void FadeIn() { 
        buttonGraphic.color = new Color(1, 1, 1, 1);
    }

    public void ToggleFade() {
        isHidden = !isHidden;

        if (isHidden)
            FadeOut();
        else
            FadeIn();
    }

}
