using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effect/Pixelate")]
public class Pixelate : MonoBehaviour
{
    public int w = 64;
    int h;

    private void Update() {
        float ratio = Camera.main.pixelHeight / Camera.main.pixelWidth;
        h = Mathf.RoundToInt(w * ratio);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}
