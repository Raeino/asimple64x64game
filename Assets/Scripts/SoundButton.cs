using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private AudioSource backMusic;

    [SerializeField] private Sprite activeImage;
    [SerializeField] private Sprite mutedImage;

    private bool muted = false;
    private Image currentImage;

    private void Awake() {
        currentImage = GetComponent<Image>();
    }

    public void Mute() {
        if (!muted)
            backMusic.Stop();
        else
            backMusic.Play();

        SwapImage();
        muted = !muted;
    }

    private void SwapImage() {
        currentImage.sprite = muted ? activeImage : mutedImage;
    }
}
