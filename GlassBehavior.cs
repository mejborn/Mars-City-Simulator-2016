﻿using UnityEngine;
using System.Collections;

public class GlassBehavior : MonoBehaviour {
    SkyLight skylight;
    Color emissionColor;
    BuildingBehavior parentBuilding;
	// Use this for initialization
    void Start()
    {
        parentBuilding = gameObject.transform.parent.GetComponent<BuildingBehavior>();

        emissionColor = new Color(1.000f, 0.949f, 0.661f);

        skylight = FindObjectsOfType<Terrain>()[0].GetComponent<SkyLight>();
    }
	
	// Update is called once per frame
	void Update () {
        if (skylight != null && parentBuilding != null && parentBuilding.isActive)
            GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", emissionColor * (Mathf.Cos(skylight.currentTimeOfDay * 2 * Mathf.PI) * 0.5f + 0.5f));
        else
            GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", Color.black);


    }
}
