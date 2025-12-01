using UnityEngine;
using Injection;
using System.Collections;

public class SetSignalToDemodulatorBitton : MonoBehaviour, IClickable
{
    [SerializeField] LineGraphRendere graphRendere;
    [Inject] SignalManager signalManager;
    [Inject] DemodulatorController demodulatorController;
    [SerializeField] ButtonAnimator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] Material baseMat;
    [SerializeField] Material okMat;
    [SerializeField] Material failMat;

    [SerializeField] GameObject okImage;
    [SerializeField] GameObject failImage;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        animator.Press();
        PlaySound();
        SetSignalToDemodulator(graphRendere.currentSignalParameters, graphRendere.currentSignalData);

        Debug.Log("Signal goes to modulator");
    }

    void SetSignalToDemodulator(SignalParameters signalParameters, SignalSO signal)
    {
        signalManager.OnSignalFromAmplifaerWasGot(signalParameters, signal);
    }

    void Fail()
    {
        StartCoroutine(ShowResult(failImage, false));
    }

    IEnumerator ShowResult(GameObject result, bool flag)
    {
        result.SetActive(true);
        meshRenderer.material = flag ? okMat : failMat; 
        yield return new WaitForSeconds(1);
        meshRenderer.material = baseMat;
        result.SetActive(false);
    }

    void Ok(SignalSO signal)
    {
        StartCoroutine(ShowResult(okImage, true));
    }

    private void OnEnable()
    {
        demodulatorController.newSignalWasSet += Ok;
        demodulatorController.wasError += Fail;
    }

    private void OnDisable()
    {
        demodulatorController.newSignalWasSet -= Ok;
        demodulatorController.wasError -= Fail;
    }
}
