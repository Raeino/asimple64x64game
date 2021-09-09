using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DotSpawner dotSpawner;
    [SerializeField] private Player player;

    [SerializeField] private Text scoreText;
    [HideInInspector] public int points = 0;

    [SerializeField] private Animator menuCanvasAnim;
    private Animator scoreTextAnimator;

    private bool canPause = true;
    private bool restarting = false;

    [SerializeField] private GameObject aGameWhere;
    [SerializeField] private GameObject pressAnyKey;
    [SerializeField] private GameObject soundButton;

    public enum GameState {
        Running,
        Pause
    }
    [HideInInspector] public GameState state;

    private void Awake() {
        Application.targetFrameRate = 60;

        scoreTextAnimator = scoreText.GetComponent<Animator>();

        state = GameState.Pause;
        Time.timeScale = 0f;

        scoreText.text = "SCORE:" + points;
    }

    private void Update() {
        // Update Game
        if (state == GameState.Running) {

            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && canPause) {
                PauseGame();
                return;
            }
        }

        // Update Pause
        if (state == GameState.Pause) {

            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0)) {

                menuCanvasAnim.SetTrigger("Resume");
                scoreTextAnimator.SetTrigger("Resume");

                state = GameState.Running;
                Time.timeScale = 1f;

                if (pressAnyKey.activeSelf)
                    pressAnyKey.SetActive(false);

                if (aGameWhere)
                    Destroy(aGameWhere);

                if (restarting) {
                    restarting = false;
                    scoreText.text = "SCORE:" + points;
                }
            }
        }
    }

    public void AddPoints(int pointsToAdd) {
        points += pointsToAdd;
        scoreText.text = "SCORE:" + points;
        scoreTextAnimator.SetTrigger("Point");

        switch(points) {
            case 5:
            case 15:
            case 25:
            case 45:
            case 60:
                dotSpawner.spawnTimeRange /= 1.25f;
                break;
            case 30:
                dotSpawner.doubleSpawn = true;
                break;
            case 110:
                dotSpawner.spawnTimeRange /= 1.15f;
                break;
        }
    }

    public IEnumerator ReduceTime() {
        Time.timeScale = 0.25f;
        // WaitForSecondsRealtime conflicts with pausing game
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 1f;
    }

    public void GameOver() {
        canPause = false;

        player.audioSource.PlayOneShot(player.redHitSounds[Random.Range(0, player.redHitSounds.Length)]);
        dotSpawner.enabled = false;
        StartCoroutine(ReduceTime());
        StartCoroutine(StartOver());
    }

    private IEnumerator StartOver() {
        yield return new WaitForSeconds(0.35f);

        player.gameObject.SetActive(false);
        foreach (Transform dot in dotSpawner.transform) {
            if (dot.gameObject)
                StartCoroutine(dot.GetComponent<Dot>().DotDestruction(2));
        }

        yield return new WaitForSecondsRealtime(0.75f);
        PauseGame();

        // Reset things
        player.gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
        if (player.disabled)
            player.Enable();
        dotSpawner.enabled = true;
        dotSpawner.ResetSpawnTimers();
        points = 0;

        pressAnyKey.SetActive(true);
        canPause = true;
        restarting = true;
    }

    private void PauseGame() {
        menuCanvasAnim.SetTrigger("Pause");
        scoreTextAnimator.SetTrigger("Pause");

        state = GameState.Pause;
        Time.timeScale = 0f;
    }
}