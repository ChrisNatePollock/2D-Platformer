﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private float speedMultiplier = 1.2f;

    private PlayerStats stats;

    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        stats.movementSpeed = Mathf.Round (stats.movementSpeed * speedMultiplier);
        UpdateValues();
    }
}