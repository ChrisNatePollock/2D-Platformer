using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    private static int _remainingLives = 3;
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

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    void Start() {
        if (cameraShake == null) {
            Debug.LogError("No camera shake in gamemaster");
        }
    }

    public void EndGame() {
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

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
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
