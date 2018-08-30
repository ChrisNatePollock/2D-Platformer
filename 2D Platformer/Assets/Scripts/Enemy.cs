using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

       

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public EnemyStats enemyStats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";

    public int moneyDrop = 10;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start() {
        enemyStats.Init();

        if(statusIndicator != null) {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if(deathParticles == null) {
            Debug.LogError("No death particles found!");
        }
    }

    void OnUpgradeMenuToggle(bool active) {
        GetComponent<EnemyAI>().enabled = !active;
    }

    public void DamageEnemy(int damage) {
        enemyStats.curHealth -= damage;
        if (enemyStats.curHealth <= 0) {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null) {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo) {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null) {
            _player.DamagePlayer(enemyStats.damage);
            DamageEnemy(9999999);
        }
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
