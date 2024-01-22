using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelSelectSphere : MonoBehaviour
{
    public SphereClickEvent sphereClicked = new SphereClickEvent();
    public int levelNo = 0;
    private readonly float bounceAmount = 0.3f;
    private readonly float bounceTime = 1f;
    private MeshRenderer renderer;

    public Material selectedMaterial;
    public Material unselectedMaterial;

    public LevelSelectSphere[] allSpheres;


    public void OnMouseDown()
    {
        foreach (LevelSelectSphere sphere in allSpheres)
            sphere.ChangeMaterial(unselectedMaterial);

        ChangeMaterial(selectedMaterial);

        sphereClicked.Invoke(levelNo);
    }

    public void Start()
    {
        HoverUp();
        allSpheres = GameObject.Find("scenery").GetComponentsInChildren<LevelSelectSphere>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void HoverUp()
    {
        LeanTween.move(gameObject, transform.position + new Vector3(0, bounceAmount, 0), bounceTime).setEaseInOutSine().setOnComplete(HoverDown);
    }

    private void HoverDown()
    {
        LeanTween.move(gameObject, transform.position + new Vector3(0, -bounceAmount, 0), bounceTime).setEaseInOutSine().setOnComplete(HoverUp);
    }

    public void ChangeMaterial(Material newMaterial)
    {
        renderer.material = newMaterial;
    }


}

public class SphereClickEvent : UnityEvent<int> { }

