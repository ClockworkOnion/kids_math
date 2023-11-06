using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCustomizer : MonoBehaviour
{
    public Material defaultMaterial;
    public Material coolMaterial;
    public Renderer carFrameRenderer;

    public void SwitchMaterial(Material mat) {
        Material[] mats = carFrameRenderer.materials;
        Debug.Log(mats.Length + " mats found, switching to " + mat.name);
        mats[0] = mat;
        carFrameRenderer.materials = mats;
    }

    public void Start()
    {
        if (GameManager.GetInstance().carMaterial is Material mat)
		    SwitchMaterial(mat);
    }
}
