using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class FireflySpawner : MonoBehaviour
{
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private GameObject fireflyPrefab;

    [SerializeField] private Text averageFrequencyText;
    [SerializeField] private Text averageInternalClockText;

    private List<Light> _fireflies;

    private float _averageFrequencies = 0;
    private float _averageInternalClocks = 0;
    
    private void Awake()
    {
        _fireflies = new List<Light>();
    }

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            _fireflies.Add(Instantiate(fireflyPrefab, transform).GetComponent<Light>());
        }
    }

    private void Update()
    {
        float frequencySum = 0;
        float internalClockSum = 0;

        foreach (var fireflyLight in _fireflies)
        {
            frequencySum = fireflyLight.FlashFrequency;
            internalClockSum = fireflyLight.InternalClock;
        }

        _averageFrequencies = frequencySum; // / _fireflies.Count;
        _averageInternalClocks = internalClockSum; // / _fireflies.Count;
        
        // update UI
        averageFrequencyText.text = $"Average Flash Frequency: {_averageFrequencies}";
        averageInternalClockText.text = $"Average Internal Clock: {_averageInternalClocks}";
        
        // close application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
