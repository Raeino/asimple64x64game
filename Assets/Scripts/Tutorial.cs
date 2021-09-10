using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject dotSpawner;

    [SerializeField] private GameObject youPressAnyKeyToStartPause;
    [SerializeField] private GameObject youUseYourMouseToDragThatBlackThing;
    [SerializeField] private GameObject youCollectWhiteDots;
    [SerializeField] private GameObject youAvoidRedDots;

    public bool firstTimeDrag = true;
    private int counter = 0;

    private void Awake() {
        int firstGame = PlayerPrefs.GetInt("firstGame", 0);
        if (firstGame == 1) {
            dotSpawner.SetActive(true);
            dotSpawner.GetComponent<DotSpawner>().canSpawnWhiteDots = true;
            PlayerPrefs.Save();
            Destroy(youUseYourMouseToDragThatBlackThing);
            Destroy(youCollectWhiteDots);
            Destroy(youAvoidRedDots);
            Destroy(gameObject);
            return;
        }

        dotSpawner.SetActive(false);
        youUseYourMouseToDragThatBlackThing.SetActive(false);
        youCollectWhiteDots.SetActive(false);
        youAvoidRedDots.SetActive(false);
    }

    private void Update() {
        // press any key to start
        if (counter == 0 && Input.anyKey && !Input.GetMouseButton(0)) {
            counter++;
            youPressAnyKeyToStartPause.SetActive(false);
            youUseYourMouseToDragThatBlackThing.SetActive(true);
        }
    }

    // Called once first black dot movement (managed in Player script) is done 
    public void FirstTimeDragged() {
        firstTimeDrag = false;
        Destroy(youUseYourMouseToDragThatBlackThing);
        counter++;

        StartCoroutine(CollectWhiteDots());
    }

    // Avoid red dots and collect white dots --> last part of the tutorial
    private IEnumerator CollectWhiteDots() {
        youAvoidRedDots.SetActive(true);
        dotSpawner.SetActive(true);
        yield return new WaitForSeconds(5f);

        youCollectWhiteDots.SetActive(true);
        dotSpawner.GetComponent<DotSpawner>().canSpawnWhiteDots = true;
        yield return new WaitForSeconds(5f);

        PlayerPrefs.SetInt("firstGame", 1);

        Destroy(youCollectWhiteDots);
        Destroy(youAvoidRedDots);
        Destroy(gameObject);
    }
}
