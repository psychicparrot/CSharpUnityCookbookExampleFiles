using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
	public Renderer theRenderer;
	public Material[] materials;

	[Space]
	public bool setRandomOnStart;

    void Start()
    {
		if (setRandomOnStart)
		{
			// choose a random material from the list
			theRenderer.material = materials[Random.Range(0, materials.Length)];
		}
    }

    public void SetMaterial(int index)
    {
		theRenderer.material = materials[index];
	}
}
