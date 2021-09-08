using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private TrailSpawner trailSpawner;
    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private bool isDragging = false;

    [HideInInspector] public bool disabled = false;

    [SerializeField] private Tutorial tutorial;
    private readonly float tutoDragTime = 3f;
    private float tutoDragTimeCounter = 0f;

    [SerializeField] AudioClip[] whiteHitSounds;
    [SerializeField] AudioClip[] redHitSounds;
    AudioSource audioSource;

    private void Awake() {
        trailSpawner = GetComponent<TrailSpawner>();
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (gameManager.state == GameManager.GameState.Running) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Drag Player
            if (Input.GetMouseButton(0)) {
                if (!isDragging && Vector2.Distance(mousePos, transform.position) <= 0.5f) {
                    isDragging = true;
                }

                // Click and hold directly on black dot ----> Instant follow
                // Click and hold away from black dot   ----> Lerp follow
                if (isDragging)
                    transform.position = mousePos + Vector3.forward * 10f;
                else {
                    transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 5f * Time.deltaTime);
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }

                // Stuff for tutorial, activates only one time
                if (tutorial) {
                    if (tutoDragTime >= tutoDragTimeCounter)
                        tutoDragTimeCounter += Time.deltaTime;
                    else 
                        tutorial.FirstTimeDragged();
                }
            }

           if (Input.GetMouseButtonUp(0) && isDragging)
                isDragging = false;
        }

        if (gameManager.state == GameManager.GameState.Pause)
            isDragging = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PointDot")) {
            gameManager.AddPoints(1);
            audioSource.PlayOneShot(whiteHitSounds[Random.Range(0, whiteHitSounds.Length)], 0.75f);
            return;
        }

        if (collision.CompareTag("DeadlyDot")) {
            StartCoroutine(gameManager.ReduceTime());
            audioSource.PlayOneShot(redHitSounds[Random.Range(0, redHitSounds.Length)], 0.75f);
            StartCoroutine(Disable(2));
        }
    }

    // Temporary disable player when he gets a red dot (sort of negative invincibility frame)
    public IEnumerator Disable(float playerDisabledTime) {
        trailSpawner.disabled = true;
        _spriteRenderer.color = new Color(255, 0, 0, 0.25f);
        _collider.enabled = false;
        disabled = true;

        yield return new WaitForSeconds(playerDisabledTime);
        Enable();
    }

    // Re-enable the player
    public void Enable() {
        disabled = false;
        trailSpawner.disabled = false;
        _spriteRenderer.color = Color.black;
        _collider.enabled = true;
    }
}