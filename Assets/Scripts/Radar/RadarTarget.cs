using System.Collections;
using UnityEngine;

public class RadarTarget : MonoBehaviour
{
    [Header("Setting up Lighting")]
    [SerializeField]
    private float fadeSpeed = 0.5f;

    private Material targetMaterial;
    [SerializeField] AudioSource audioSource;
    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }

    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            targetMaterial = renderer.material;
            SetTransparency(0f); 
        }
    }

    public void LightUp()
    {
        PlaySound();

        StopAllCoroutines();

        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        
        SetTransparency(1f); 

        float currentAlpha = 1f;
        while (currentAlpha > 0f)
        {
            currentAlpha -= fadeSpeed * Time.deltaTime;
            currentAlpha = Mathf.Max(currentAlpha, 0f); 

            SetTransparency(currentAlpha);

            yield return null; 
        }
    }

    private void SetTransparency(float alpha)
    {
        if (targetMaterial != null)
        {
            Color color = targetMaterial.color;
            color.a = alpha;
            targetMaterial.color = color;

            if (targetMaterial.IsKeywordEnabled("_EMISSION"))
            {
                Color emissionColor = targetMaterial.GetColor("_EmissionColor");
                emissionColor *= alpha;        
                targetMaterial.SetColor("_EmissionColor", emissionColor);
            }
        }


    }
}