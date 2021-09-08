using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpawnFreq;
    private float dotSpawnTimer = 0f;
    [HideInInspector] public bool disabled = false;

    private Vector3 lastPos;

    private void Update() {
        SpawnTrailPart();
    }

    private void SpawnTrailPart() {
        if (lastPos == transform.position)
            return;

        if (dotSpawnTimer > dotSpawnFreq)
            dotSpawnTimer += Time.deltaTime;
        else {
            dotSpawnTimer = 0f;
            // Spawn at position z = 1
            GameObject dot = Instantiate(dotPrefab, transform.position + Vector3.forward * 11, Quaternion.identity);

            if (disabled)
                dot.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);
        }

        lastPos = transform.position;
    }
}