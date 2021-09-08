using UnityEngine;

public class RotatingThing : MonoBehaviour
{
    private float rotationForce;

    private void Awake() {
        transform.eulerAngles += Vector3.forward * Random.Range(0, 90f);
        rotationForce = Random.Range(-100f, 100f);
    }

    private void Update() {
        transform.eulerAngles += Vector3.forward * rotationForce * Time.deltaTime;
    }
}
