using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives {
        get { return _remainingLives; }
    }

    void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }
    
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public string spawnSoundName;

    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    // cache
    private AudioManager audioManager;

    void Start() {
        if (cameraShake == null) {
            Debug.LogError("No camera shake in gamemaster");
        }

        _remainingLives = maxLives;

        //caching
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("FREAK OUT! No AudioManager found in the scene!");
        }
    }

    public void EndGame() {
        audioManager.PlaySound(gameOverSoundName);
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer() {
        audioManager.PlaySound(spawnSoundName);
        yield return new WaitForSeconds(spawnDelay);
        // here we would place a respawn sound but our spawnSoundName has both the countdown and the respawn sound together
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        _remainingLives -= 1;
        if(_remainingLives <= 0) {
            gm.EndGame();
        }
        else {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
        
    }

    public static void KillEnemy(Enemy enemy) {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy) {
        // sound
        audioManager.PlaySound(_enemy.deathSoundName);

        // particles
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;

        //camera shake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);

        Destroy(_enemy.gameObject);
    }
}
