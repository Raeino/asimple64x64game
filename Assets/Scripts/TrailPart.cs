using UnityEngine;

public class TrailPart : MonoBehaviour
{
    [SerializeField] private float reduceSizeSpeed;
    [SerializeField] private float reduceAlphaSpeed;
    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.localScale -= Vector3.one * reduceSizeSpeed * Time.unscaledDeltaTime;
        sr.color -= Color.black * reduceAlphaSpeed * Time.unscaledDeltaTime;

        if (transform.localScale.x <= 0)
            Destroy(gameObject);
    }
}
