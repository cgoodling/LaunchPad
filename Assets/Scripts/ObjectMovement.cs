﻿using UnityEngine;

[DisallowMultipleComponent]
public class ObjectMovement : MonoBehaviour {

    [SerializeField] Vector3 movementVector;
	[SerializeField] float period = 2f;

	float movementFactor;
    Vector3 startingPos;

    // Use this for initialization
    void Start() {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (period <= Mathf.Epsilon) { return; } 
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; // about 6.28 radian
        float rawSineWave = Mathf.Sin(cycles * tau); // goes from -1 to 1

        movementFactor = (rawSineWave / 2f) + 0.5f; // adjust amplitude, 0 to 1
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}