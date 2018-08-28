using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public PlayerStats playerStats = new PlayerStats();

    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "DamageVoice";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start() {
        playerStats.Init();
        if(statusIndicator == null) {
            Debug.LogError("No status indicator referenced on player");
        }else {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("No audioManager found in scene");
        }
    }

    void Update () {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }
    }

    public void DamagePlayer(int damage) {
        playerStats.curHealth -= damage;
        if (playerStats.curHealth <= 0) {
            //play death sound
            audioManager.PlaySound(deathSoundName);

            GameMaster.KillPlayer(this);
        }else
        {
            // play damage sound
            audioManager.PlaySound(damageSoundName);
        }

        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }
}
