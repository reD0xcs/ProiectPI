using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;
    public float fadeSpeed = 2f; 

    void Start()
    {
        flashImage.enabled = false;
    }

    public void FlashRed()
    {
        StartCoroutine(Flash(new Color(1f, 0f, 0f, 0.5f), new Color(1f, 0f, 0f, 0.4f), 0.5f)); 
    }

    public void FlashGreen()
    {
        StartCoroutine(Flash(new Color(0f, 1f, 0f, 0f), new Color(0f, 1f, 0f, 0.1f), 0.5f)); 
    }

    IEnumerator Flash(Color startColor, Color endColor, float duration)
    {
        flashImage.color = startColor;
        flashImage.enabled = true;

        float elapsedTime = 0f;

        // Fade-in
        while (elapsedTime < duration / 2)
        {
            flashImage.color = Color.Lerp(startColor, endColor, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for the remaining duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Gradual fade-out
        while (flashImage.color.a > 0)
        {
            flashImage.color = new Color(endColor.r, endColor.g, endColor.b, flashImage.color.a - fadeSpeed * Time.deltaTime);
            yield return null;
        }

        flashImage.enabled = false;
    }
}