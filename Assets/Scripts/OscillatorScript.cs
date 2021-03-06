﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class OscillatorScript : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    // todo rmove form inspector later 
    float movementFactor;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
