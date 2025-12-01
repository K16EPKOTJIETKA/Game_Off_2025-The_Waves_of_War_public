using DG.Tweening;
using UnityEngine;

public class FrequencyRegulator : MonoBehaviour, IShowableUI
{
    int step = 360 / 240;
    int stepsCount = 0;
    [SerializeField] AmplifaerSignalSettings amplifaerSignalSettings;
    [SerializeField] GameObject ui;
    [SerializeField] Transform rotator;
    AudioSource audioSource;
    Tween rotateTween;
    private Vector3 baseRotation;
    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }

    public void RotateRegulatorRigt()
    {
        if (stepsCount == 240) return;
        RotateToStep();
        PlaySound();
        stepsCount++;
        amplifaerSignalSettings.signalFrequency = stepsCount + 10;
    }
    void RotateToStep()
    {
        rotateTween?.Kill();

        float z = baseRotation.z + (-stepsCount * step);

        rotateTween = rotator.DOLocalRotate(
            new Vector3(baseRotation.x, baseRotation.y, z),
            0.08f
        ).SetEase(Ease.OutQuad);
    }

    public void RotateRegulatorLeft()
    {
        if (stepsCount == 0) return;
        RotateToStep();
        PlaySound();
        stepsCount--;
        amplifaerSignalSettings.signalFrequency = stepsCount + 10;
    }

    public void ShowUI()
    {
        ui.SetActive(true);
    }

    public void HideUI()
    {
        ui.SetActive(false);
    }

    private void Awake()
    {
        HideUI();
        audioSource = GetComponent<AudioSource>();
        baseRotation = rotator.localEulerAngles;
    }
}
