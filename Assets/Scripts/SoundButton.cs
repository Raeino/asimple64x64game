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
        backMusic.SetActive(!muted);
        currentImage.sprite = muted ? activeImage : mutedImage;
        muted = !muted;
    }
}
