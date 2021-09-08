using UnityEngine;

public class DotSpawner : MonoBehaviour {
    [SerializeField] private Vector2 xSpawnPosRange;
    [SerializeField] private float ySpawnPos;

    public Vector2 spawnTimeRange;
    private float spawnTimeCounter;
    private float spawnTime;

    private float spawnDeadlyTimeCounter;
    private float spawnDeadlyTime;

    private Vector2 firstSpawnTimeRange;

    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject deadlyDotPrefab;

    [HideInInspector] public Animator cameraAnimator;

    [HideInInspector] public bool doubleSpawn = false;
    [HideInInspector] public bool canSpawnWhiteDots = false;

    private Vector2 spawnPos = Vector2.zero;
    private Vector2 previousSpawnPos = Vector2.zero;

    private void Awake() {
        spawnTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
        spawnDeadlyTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);

        cameraAnimator = Camera.main.GetComponent<Animator>();

        firstSpawnTimeRange = spawnTimeRange;
    }

    private void Update() {
        // Red dot
        if (spawnDeadlyTimeCounter < spawnDeadlyTime)
            spawnDeadlyTimeCounter += Time.deltaTime;
        else {
            spawnDeadlyTimeCounter = 0f;
            SpawnDeadlyDot();

            if (doubleSpawn && Random.Range(0, 2) == 0)
                SpawnDeadlyDot();
        }

        if (!canSpawnWhiteDots)
            return;

        // White dot
        if (spawnTimeCounter < spawnTime)
            spawnTimeCounter += Time.deltaTime;
        else {
            spawnTimeCounter = 0f;
            SpawnDot();

            if (doubleSpawn && Random.Range(0, 3) == 0)
                SpawnDot();
        }
    }

    private void SpawnDot() {
        spawnTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);

        // Make sure two dots don't spawn too much next to each other
        while (Mathf.Abs(previousSpawnPos.x - spawnPos.x) < 1f) {
            float xSpawnPos = Random.Range(xSpawnPosRange.x, xSpawnPosRange.y);
            spawnPos = new Vector2(xSpawnPos, ySpawnPos);
        }

        Instantiate(dotPrefab, spawnPos, Quaternion.identity)
            .transform.SetParent(transform);
        previousSpawnPos = spawnPos;
    }

    private void SpawnDeadlyDot() {
        // Red dots spawn slightly less frequently than white dots
        spawnDeadlyTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y) * 1.25f;

        Vector2 spawnPos = Vector2.zero;
        int edge = Random.Range(0, 4);

        switch(edge) {
            case 0: // up
                spawnPos = new Vector2(Random.Range(-5f, 5f), 5.1f);
                break;
            case 1: // down
                spawnPos = new Vector2(Random.Range(-5f, 5f), -5.1f);
                break;
            case 2: // left
                spawnPos = new Vector2(Random.Range(5f, -5f), -5.1f);
                break;
            case 3: // right
                spawnPos = new Vector2(Random.Range(5f, -5f), 5.1f);
                break;
        }

        GameObject deadlyDot = Instantiate(deadlyDotPrefab, spawnPos, Quaternion.identity);
        deadlyDot.transform.SetParent(transform);
        deadlyDot.GetComponent<Dot>().deadly = true;
    }

    public void ResetSpawnTimers() {
        spawnTimeRange = firstSpawnTimeRange;
        doubleSpawn = false;
    }
}