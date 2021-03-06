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

        // Keep muted stat from last time
        if (PlayerPrefs.GetInt("muted", 0) == 1)
            Mute();
    }

    public void Mute() {
        muted = !muted;
        backMusic.mute = muted;
        currentImage.sprite = muted ? mutedImage : activeImage;

        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}