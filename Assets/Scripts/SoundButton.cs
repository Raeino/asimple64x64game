using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private GameObject backMusic;

    [SerializeField] private Sprite activeImage;
    [SerializeField] private Sprite mutedImage;

    private bool muted = false;
    private Image currentImage;

    private void Awake() {
        currentImage = GetComponent<Image>();
    }

    public void Mute() {
        muted = !muted;
        backMusic.SetActive(!muted);
        currentImage.sprite = muted ? mutedImage : activeImage;
    }
}