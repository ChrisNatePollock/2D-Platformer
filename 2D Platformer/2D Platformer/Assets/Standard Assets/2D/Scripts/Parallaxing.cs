﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;             // Array of all backgrounds and foreground to be parallaxed
    private float[] parallaxScales;             // The proportion of the camera's movement to move the background by
    public float smoothing = 1f;                // How smooth the parallax is going to be must be set above ZERO

    private Transform cam;                      // Reference to main camera's transfom
    private Vector3 previousCamPos;             // Position of camera in the previous frame

    // Called before start, great for references
    void Awake() {
        // Set up camera reference
        cam = Camera.main.transform;
    }

	// Use this for initialization
	void Start () {
        // The previous frame had the current frames camera position
        previousCamPos = cam.position;

        // Assiging corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < backgrounds.Length; ++i) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //for each background
		for(int i = 0; i < backgrounds.Length; ++i) {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // Set a target x position which is the current position + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a target position which is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
	}
}