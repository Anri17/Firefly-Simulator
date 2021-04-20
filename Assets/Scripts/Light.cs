using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class Light : MonoBehaviour
{
    [SerializeField] private Light2D light;

    public float FlashFrequency;

    public bool FlashedLight = false;

    public float InternalClock = 0;

    private void Start()
    {
        // values are in seconds
        FlashFrequency = Random.Range(4, 8);

        // this prevents all the fireflies to flash at the same time at the start
        InternalClock = Random.Range(0, FlashFrequency);
    }

    private void Update()
    {
        InternalClock += Time.deltaTime;
        if (InternalClock >= FlashFrequency)
        {
            FlashLight();
            InternalClock -= FlashFrequency;
        }
    }

    private void FlashLight()
    {
        StartCoroutine(FlashLightCoroutine());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Firefly"))
        {
            Light otherLight = other.GetComponent<Light>();

            if (otherLight.FlashedLight)
            {
                // average out the Internal clock
                float halfFrequency = FlashFrequency / 2.0f;
                if (InternalClock > halfFrequency)
                {
                    InternalClock += (FlashFrequency - InternalClock) / 100.0f;
                }
                else if (InternalClock < halfFrequency)
                {
                    InternalClock -= InternalClock / 100.0f;
                }
                
                // average out the Flash Frequency
                float flashFrequencyOffset = otherLight.FlashFrequency - FlashFrequency;

                FlashFrequency += flashFrequencyOffset / 1.5f;
            }
        }
    }

    private IEnumerator FlashLightCoroutine()
    {
        // Flash this for only a few frames, as it should only be detected by nearby fireflies
        FlashedLight = true;
        yield return new WaitForFixedUpdate();
        FlashedLight = false;

        /*
         * Complicated(not really) calculations for defining the light intensity 
         * 1 - 3
         * 0 - 0.5
         *
         * y = ax + b
         * 
         * 3 = a1 + b
         * 0.5 = a0 + b
         * 
         * I = 5/2x + 1/2
         * 
         */
        for (float t = 1; t > 0; t -= 0.05f)
        {
            light.intensity = ((5.0f / 2.0f) * t) + (1.0f / 4.0f);

            yield return null;
        }
    }
}