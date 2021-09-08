using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour 
{
    [SerializeField] private GameObject dotExplosionParticles;
    [SerializeField] private GameObject dotDestroyedPrefab;
    
    [SerializeField] private DotSpawner dotSpawner;
    private CircleCollider2D coll;
    private SpriteRenderer sr;

    public bool deadly;
    private Vector3 deadlyDotDirection;

    private Transform player;

    [SerializeField] private Vector2 moveSpeedRange;
    private float moveSpeed;
    [SerializeField] private float deadlyFollowPlayerForce;

    [SerializeField] private Sprite defaultCircle;

    private bool isDestroying = false;

    private void Awake() {
        coll = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        // Referencing dotSpanwer and Player here with Find since they're not prefabs
        dotSpawner = GameObject.Find("Dot Spawner").GetComponent<DotSpawner>();
        GameObject playerGO = GameObject.Find("Player");

        if (playerGO) {
            player = playerGO.transform;

            // Red dots will follow the player
            // White dots will fall down
            if (deadly)
                deadlyDotDirection = player.position - transform.position;
            else {
                transform.localScale *= Random.Range(0.75f, 1.25f);
                moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
            }
        }
    }

    private void Update() {
        if (isDestroying)
            return;

        // Quantity control for red dots (destroy if dot is out of position's bounds)
        if (deadly) {
            if (transform.position.x >= 5.5f || transform.position.x <= -5.5f ||
                transform.position.y >= 5.5f || transform.position.y <= -5.5f)
                Destroy(gameObject);
        }
 
        // Different movement for red and white dots
         transform.position +=  deadly ? deadlyDotDirection * deadlyFollowPlayerForce * Time.deltaTime :
            Vector3.down * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {        
        if (!deadly && collision.CompareTag("Death")) {
            StartCoroutine(DotDestruction(5));

            // Calls GameOver: gets component here so that not every single dot has to  
            // get GameManager component (it has to happen once per game)
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }

        if (collision.CompareTag("Player") && collision.transform.localScale.x >= .095f) {
            dotSpawner.cameraAnimator.SetTrigger("Shake");
            StartCoroutine(DotDestruction(2));
        }
    }

    public IEnumerator DotDestruction(int scaleMultiplier) {
        Instantiate(dotExplosionParticles, transform.position, Quaternion.identity);
        coll.enabled = false;
        isDestroying = true;
        sr.sprite = defaultCircle;
        transform.localScale *= scaleMultiplier;
        transform.position += Vector3.forward * 5f;
        yield return new WaitForSecondsRealtime(0.15f);
        if (sr)
            sr.color = Color.black;
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(gameObject);
    }
}